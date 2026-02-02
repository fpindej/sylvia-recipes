using Microsoft.EntityFrameworkCore;
using Recipes.Application.Features.Recipes;
using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Domain;
using Recipes.Infrastructure.Persistence;
using Recipes.Infrastructure.Persistence.Extensions;

namespace Recipes.Infrastructure.Features.Recipes.Services;

internal class TagService(RecipesDbContext dbContext) : ITagService
{
    private const double SimilarityThreshold = 0.2;

    public async Task<Result<IReadOnlyList<TagOutput>>> GetAllAsync(
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Tags
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(t => t.Name.ToLower().Similarity(term) > SimilarityThreshold);
        }

        var tags = await query
            .OrderBy(t => t.TagType)
            .ThenBy(t => t.Name)
            .Select(t => new TagOutput(t.Id, t.Name, t.TagType))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<TagOutput>>.Success(tags);
    }
}
