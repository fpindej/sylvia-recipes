using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Recipes.Application.Features.Recipes;
using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Application.Shared;
using Recipes.Domain;
using Recipes.Domain.Entities;
using Recipes.Domain.Enums;
using Recipes.Infrastructure.Persistence;
using Recipes.Infrastructure.Persistence.Extensions;

namespace Recipes.Infrastructure.Features.Recipes.Services;

internal class RecipeService(
    RecipesDbContext dbContext,
    ILogger<RecipeService> logger) : IRecipeService
{
    private const double SimilarityThreshold = 0.1;

    public async Task<Result<Guid>> CreateAsync(CreateRecipeInput input, CancellationToken cancellationToken = default)
    {
        var recipe = new Recipe(
            title: input.Title,
            instructions: input.Instructions,
            description: input.Description,
            prepTimeMinutes: input.PrepTimeMinutes,
            cookTimeMinutes: input.CookTimeMinutes,
            servings: input.Servings,
            proteinGrams: input.ProteinGrams,
            isTried: input.IsTried,
            sourceUrl: input.SourceUrl,
            imageUrl: input.ImageUrl,
            notes: input.Notes,
            workspaceNeeded: input.WorkspaceNeeded,
            timeCategory: input.TimeCategory,
            messiness: input.Messiness);

        dbContext.Recipes.Add(recipe);

        // Handle tags (inline creation)
        if (input.Tags is { Count: > 0 })
        {
            foreach (var tagInput in input.Tags)
            {
                var tag = await GetOrCreateTagAsync(tagInput.Name, tagInput.TagType, cancellationToken);
                recipe.RecipeTags.Add(new RecipeTag(recipe.Id, tag.Id));
            }
        }

        // Handle equipment (inline creation)
        if (input.EquipmentNames is { Count: > 0 })
        {
            foreach (var equipmentName in input.EquipmentNames)
            {
                var equipment = await GetOrCreateEquipmentAsync(equipmentName, cancellationToken);
                recipe.RecipeEquipment.Add(new RecipeEquipment(recipe.Id, equipment.Id));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created recipe {RecipeId} with title {Title}", recipe.Id, recipe.Title);

        return Result<Guid>.Success(recipe.Id);
    }

    public async Task<Result<RecipeOutput>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var recipe = await dbContext.Recipes
            .AsNoTracking()
            .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
            .Include(r => r.RecipeEquipment)
                .ThenInclude(re => re.Equipment)
            .Where(r => !r.IsDeleted)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe is null)
        {
            return Result<RecipeOutput>.Failure("Recipe not found.");
        }

        return Result<RecipeOutput>.Success(MapToOutput(recipe));
    }

    public async Task<Result<PaginatedOutput<RecipeOutput>>> GetFilteredAsync(
        RecipeFilterInput filter,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Recipes
            .AsNoTracking()
            .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
            .Include(r => r.RecipeEquipment)
                .ThenInclude(re => re.Equipment)
            .Where(r => !r.IsDeleted)
            .AsQueryable();

        // Apply full-text search using pg_trgm similarity
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(r =>
                r.Title.ToLower().Similarity(searchTerm) > SimilarityThreshold ||
                (r.Description != null && r.Description.ToLower().Similarity(searchTerm) > SimilarityThreshold));
        }

        // Filter by tried status
        if (filter.IsTried.HasValue)
        {
            query = query.Where(r => r.IsTried == filter.IsTried.Value);
        }

        // Filter by cuisines (tag type = Cuisine)
        if (filter.Cuisines is { Count: > 0 })
        {
            var cuisineNames = filter.Cuisines.Select(c => c.ToLower()).ToList();
            query = query.Where(r => r.RecipeTags.Any(rt =>
                rt.Tag.TagType == TagType.Cuisine &&
                cuisineNames.Contains(rt.Tag.Name.ToLower())));
        }

        // Filter by types (tag type = Type)
        if (filter.Types is { Count: > 0 })
        {
            var typeNames = filter.Types.Select(t => t.ToLower()).ToList();
            query = query.Where(r => r.RecipeTags.Any(rt =>
                rt.Tag.TagType == TagType.Type &&
                typeNames.Contains(rt.Tag.Name.ToLower())));
        }

        // Filter by equipment
        if (filter.EquipmentNames is { Count: > 0 })
        {
            var equipmentNames = filter.EquipmentNames.Select(e => e.ToLower()).ToList();
            query = query.Where(r => r.RecipeEquipment.Any(re =>
                equipmentNames.Contains(re.Equipment.Name.ToLower())));
        }

        // Filter by workspace needed
        if (filter.WorkspaceNeeded.HasValue)
        {
            query = query.Where(r => r.WorkspaceNeeded == filter.WorkspaceNeeded.Value);
        }

        // Filter by time category
        if (filter.TimeCategory.HasValue)
        {
            query = query.Where(r => r.TimeCategory == filter.TimeCategory.Value);
        }

        // Filter by messiness
        if (filter.Messiness.HasValue)
        {
            query = query.Where(r => r.Messiness == filter.Messiness.Value);
        }

        // Filter by minimum protein
        if (filter.MinProteinGrams.HasValue)
        {
            query = query.Where(r => r.ProteinGrams >= filter.MinProteinGrams.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and ordering
        var recipes = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        var items = recipes.Select(MapToOutput).ToList();

        return Result<PaginatedOutput<RecipeOutput>>.Success(
            new PaginatedOutput<RecipeOutput>(items, totalCount, filter.PageNumber, filter.PageSize));
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateRecipeInput input, CancellationToken cancellationToken = default)
    {
        var recipe = await dbContext.Recipes
            .Include(r => r.RecipeTags)
            .Include(r => r.RecipeEquipment)
            .Where(r => !r.IsDeleted)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe is null)
        {
            return Result.Failure("Recipe not found.");
        }

        recipe.Update(
            title: input.Title,
            instructions: input.Instructions,
            description: input.Description,
            prepTimeMinutes: input.PrepTimeMinutes,
            cookTimeMinutes: input.CookTimeMinutes,
            servings: input.Servings,
            proteinGrams: input.ProteinGrams,
            isTried: input.IsTried,
            sourceUrl: input.SourceUrl,
            imageUrl: input.ImageUrl,
            notes: input.Notes,
            workspaceNeeded: input.WorkspaceNeeded,
            timeCategory: input.TimeCategory,
            messiness: input.Messiness);

        // Update tags if provided
        if (input.Tags is not null)
        {
            // Remove existing tags
            recipe.RecipeTags.Clear();

            // Add new tags
            foreach (var tagInput in input.Tags)
            {
                var tag = await GetOrCreateTagAsync(tagInput.Name, tagInput.TagType, cancellationToken);
                recipe.RecipeTags.Add(new RecipeTag(recipe.Id, tag.Id));
            }
        }

        // Update equipment if provided
        if (input.EquipmentNames is not null)
        {
            // Remove existing equipment
            recipe.RecipeEquipment.Clear();

            // Add new equipment
            foreach (var equipmentName in input.EquipmentNames)
            {
                var equipment = await GetOrCreateEquipmentAsync(equipmentName, cancellationToken);
                recipe.RecipeEquipment.Add(new RecipeEquipment(recipe.Id, equipment.Id));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Updated recipe {RecipeId}", id);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var recipe = await dbContext.Recipes
            .Where(r => !r.IsDeleted)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe is null)
        {
            return Result.Failure("Recipe not found.");
        }

        recipe.SoftDelete();
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Soft deleted recipe {RecipeId}", id);

        return Result.Success();
    }

    public async Task<Result> MarkAsTriedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var recipe = await dbContext.Recipes
            .Where(r => !r.IsDeleted)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe is null)
        {
            return Result.Failure("Recipe not found.");
        }

        recipe.MarkAsTried();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> MarkAsNotTriedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var recipe = await dbContext.Recipes
            .Where(r => !r.IsDeleted)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe is null)
        {
            return Result.Failure("Recipe not found.");
        }

        recipe.MarkAsNotTried();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Tag> GetOrCreateTagAsync(string name, TagType tagType, CancellationToken cancellationToken)
    {
        var normalizedName = name.Trim().ToLower();

        var existingTag = await dbContext.Tags
            .FirstOrDefaultAsync(t =>
                t.Name.ToLower() == normalizedName &&
                t.TagType == tagType &&
                !t.IsDeleted,
                cancellationToken);

        if (existingTag is not null)
        {
            return existingTag;
        }

        var newTag = new Tag(name.Trim(), tagType);
        dbContext.Tags.Add(newTag);

        return newTag;
    }

    private async Task<Equipment> GetOrCreateEquipmentAsync(string name, CancellationToken cancellationToken)
    {
        var normalizedName = name.Trim().ToLower();

        var existingEquipment = await dbContext.Equipment
            .FirstOrDefaultAsync(e =>
                e.Name.ToLower() == normalizedName &&
                !e.IsDeleted,
                cancellationToken);

        if (existingEquipment is not null)
        {
            return existingEquipment;
        }

        var newEquipment = new Equipment(name.Trim());
        dbContext.Equipment.Add(newEquipment);

        return newEquipment;
    }

    private static RecipeOutput MapToOutput(Recipe recipe)
    {
        return new RecipeOutput(
            Id: recipe.Id,
            Title: recipe.Title,
            Instructions: recipe.Instructions,
            Description: recipe.Description,
            PrepTimeMinutes: recipe.PrepTimeMinutes,
            CookTimeMinutes: recipe.CookTimeMinutes,
            Servings: recipe.Servings,
            ProteinGrams: recipe.ProteinGrams,
            IsTried: recipe.IsTried,
            SourceUrl: recipe.SourceUrl,
            ImageUrl: recipe.ImageUrl,
            Notes: recipe.Notes,
            WorkspaceNeeded: recipe.WorkspaceNeeded,
            TimeCategory: recipe.TimeCategory,
            Messiness: recipe.Messiness,
            Tags: recipe.RecipeTags
                .Where(rt => !rt.Tag.IsDeleted)
                .Select(rt => new TagOutput(rt.Tag.Id, rt.Tag.Name, rt.Tag.TagType))
                .ToList(),
            Equipment: recipe.RecipeEquipment
                .Where(re => !re.Equipment.IsDeleted)
                .Select(re => new EquipmentOutput(re.Equipment.Id, re.Equipment.Name))
                .ToList(),
            CreatedAt: recipe.CreatedAt,
            UpdatedAt: recipe.UpdatedAt);
    }
}
