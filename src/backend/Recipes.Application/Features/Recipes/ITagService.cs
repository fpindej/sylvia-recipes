using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Domain;

namespace Recipes.Application.Features.Recipes;

/// <summary>
/// Service interface for tag management operations.
/// </summary>
public interface ITagService
{
    /// <summary>
    /// Gets all tags, optionally filtered by search term.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter tags by name.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing the list of tags.</returns>
    Task<Result<IReadOnlyList<TagOutput>>> GetAllAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
}
