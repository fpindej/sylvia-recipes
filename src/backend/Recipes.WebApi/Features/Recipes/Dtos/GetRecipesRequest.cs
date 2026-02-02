using JetBrains.Annotations;
using Recipes.Domain.Enums;
using Recipes.WebApi.Shared;

namespace Recipes.WebApi.Features.Recipes.Dtos;

/// <summary>
/// Request to get a filtered list of recipes.
/// </summary>
public class GetRecipesRequest : PaginatedRequest
{
    /// <summary>
    /// Search term for full-text search on title and description.
    /// </summary>
    public string? SearchTerm { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by tried status.
    /// </summary>
    public bool? IsTried { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by cuisine tags (comma-separated).
    /// </summary>
    public string? Cuisines { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by type tags (comma-separated).
    /// </summary>
    public string? Types { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by equipment names (comma-separated).
    /// </summary>
    public string? Equipment { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by workspace needed.
    /// </summary>
    public WorkspaceNeeded? WorkspaceNeeded { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by time category.
    /// </summary>
    public TimeCategory? TimeCategory { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by messiness level.
    /// </summary>
    public Messiness? Messiness { get; [UsedImplicitly] set; }

    /// <summary>
    /// Filter by minimum protein grams.
    /// </summary>
    public decimal? MinProteinGrams { get; [UsedImplicitly] set; }
}
