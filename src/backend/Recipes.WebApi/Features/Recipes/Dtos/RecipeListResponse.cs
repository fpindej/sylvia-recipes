using JetBrains.Annotations;
using Recipes.WebApi.Shared;

namespace Recipes.WebApi.Features.Recipes.Dtos;

/// <summary>
/// Response containing a paginated list of recipes.
/// </summary>
public class RecipeListResponse : PaginatedResponse
{
    /// <summary>
    /// The list of recipes.
    /// </summary>
    public List<RecipeResponse> Items { get; [UsedImplicitly] init; } = [];
}
