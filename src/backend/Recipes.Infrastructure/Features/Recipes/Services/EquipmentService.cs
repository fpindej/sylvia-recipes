using Microsoft.EntityFrameworkCore;
using Recipes.Application.Features.Recipes;
using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Domain;
using Recipes.Infrastructure.Persistence;
using Recipes.Infrastructure.Persistence.Extensions;

namespace Recipes.Infrastructure.Features.Recipes.Services;

internal class EquipmentService(RecipesDbContext dbContext) : IEquipmentService
{
    private const double SimilarityThreshold = 0.2;

    public async Task<Result<IReadOnlyList<EquipmentOutput>>> GetAllAsync(
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Equipment
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Similarity(term) > SimilarityThreshold);
        }

        var equipment = await query
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentOutput(e.Id, e.Name))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<EquipmentOutput>>.Success(equipment);
    }
}
