using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Domain;

namespace Recipes.Application.Features.Recipes;

/// <summary>
/// Service interface for equipment management operations.
/// </summary>
public interface IEquipmentService
{
    /// <summary>
    /// Gets all equipment, optionally filtered by search term.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter equipment by name.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing the list of equipment.</returns>
    Task<Result<IReadOnlyList<EquipmentOutput>>> GetAllAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
}
