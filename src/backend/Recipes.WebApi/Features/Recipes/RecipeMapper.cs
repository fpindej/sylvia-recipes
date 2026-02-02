using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Application.Shared;
using Recipes.WebApi.Features.Recipes.Dtos;

namespace Recipes.WebApi.Features.Recipes;

/// <summary>
/// Mapper for converting between Recipe DTOs.
/// </summary>
public static class RecipeMapper
{
    /// <summary>
    /// Converts a CreateRecipeRequest to CreateRecipeInput.
    /// </summary>
    public static CreateRecipeInput ToInput(this CreateRecipeRequest request) => new(
        Title: request.Title,
        Instructions: request.Instructions,
        Description: request.Description,
        PrepTimeMinutes: request.PrepTimeMinutes,
        CookTimeMinutes: request.CookTimeMinutes,
        Servings: request.Servings,
        ProteinGrams: request.ProteinGrams,
        IsTried: request.IsTried,
        SourceUrl: request.SourceUrl,
        ImageUrl: request.ImageUrl,
        Notes: request.Notes,
        WorkspaceNeeded: request.WorkspaceNeeded,
        TimeCategory: request.TimeCategory,
        Messiness: request.Messiness,
        Tags: request.Tags?.Select(t => new TagInput(t.Name, t.TagType)).ToList(),
        EquipmentNames: request.EquipmentNames
    );

    /// <summary>
    /// Converts an UpdateRecipeRequest to UpdateRecipeInput.
    /// </summary>
    public static UpdateRecipeInput ToInput(this UpdateRecipeRequest request) => new(
        Title: request.Title,
        Instructions: request.Instructions,
        Description: request.Description,
        PrepTimeMinutes: request.PrepTimeMinutes,
        CookTimeMinutes: request.CookTimeMinutes,
        Servings: request.Servings,
        ProteinGrams: request.ProteinGrams,
        IsTried: request.IsTried,
        SourceUrl: request.SourceUrl,
        ImageUrl: request.ImageUrl,
        Notes: request.Notes,
        WorkspaceNeeded: request.WorkspaceNeeded,
        TimeCategory: request.TimeCategory,
        Messiness: request.Messiness,
        Tags: request.Tags?.Select(t => new TagInput(t.Name, t.TagType)).ToList(),
        EquipmentNames: request.EquipmentNames
    );

    /// <summary>
    /// Converts a GetRecipesRequest to RecipeFilterInput.
    /// </summary>
    public static RecipeFilterInput ToInput(this GetRecipesRequest request) => new(
        SearchTerm: request.SearchTerm,
        IsTried: request.IsTried,
        Cuisines: ParseCommaSeparated(request.Cuisines),
        Types: ParseCommaSeparated(request.Types),
        EquipmentNames: ParseCommaSeparated(request.Equipment),
        WorkspaceNeeded: request.WorkspaceNeeded,
        TimeCategory: request.TimeCategory,
        Messiness: request.Messiness,
        MinProteinGrams: request.MinProteinGrams,
        PageNumber: request.PageNumber,
        PageSize: request.PageSize
    );

    /// <summary>
    /// Converts a RecipeOutput to RecipeResponse.
    /// </summary>
    public static RecipeResponse ToResponse(this RecipeOutput output) => new()
    {
        Id = output.Id,
        Title = output.Title,
        Instructions = output.Instructions,
        Description = output.Description,
        PrepTimeMinutes = output.PrepTimeMinutes,
        CookTimeMinutes = output.CookTimeMinutes,
        Servings = output.Servings,
        ProteinGrams = output.ProteinGrams,
        IsTried = output.IsTried,
        SourceUrl = output.SourceUrl,
        ImageUrl = output.ImageUrl,
        Notes = output.Notes,
        WorkspaceNeeded = output.WorkspaceNeeded,
        TimeCategory = output.TimeCategory,
        Messiness = output.Messiness,
        Tags = output.Tags.Select(t => new TagResponse
        {
            Id = t.Id,
            Name = t.Name,
            TagType = t.TagType
        }).ToList(),
        Equipment = output.Equipment.Select(e => new EquipmentResponse
        {
            Id = e.Id,
            Name = e.Name
        }).ToList(),
        CreatedAt = output.CreatedAt,
        UpdatedAt = output.UpdatedAt
    };

    /// <summary>
    /// Converts a PaginatedOutput of RecipeOutput to RecipeListResponse.
    /// </summary>
    public static RecipeListResponse ToResponse(this PaginatedOutput<RecipeOutput> output) => new()
    {
        Items = output.Items.Select(r => r.ToResponse()).ToList(),
        TotalCount = output.TotalCount,
        PageNumber = output.PageNumber,
        PageSize = output.PageSize
    };

    /// <summary>
    /// Converts a TagOutput to TagResponse.
    /// </summary>
    public static TagResponse ToResponse(this TagOutput output) => new()
    {
        Id = output.Id,
        Name = output.Name,
        TagType = output.TagType
    };

    /// <summary>
    /// Converts an EquipmentOutput to EquipmentResponse.
    /// </summary>
    public static EquipmentResponse ToResponse(this EquipmentOutput output) => new()
    {
        Id = output.Id,
        Name = output.Name
    };

    private static IReadOnlyList<string>? ParseCommaSeparated(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
}
