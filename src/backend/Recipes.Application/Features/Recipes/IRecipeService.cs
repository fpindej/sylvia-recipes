using Recipes.Application.Features.Recipes.Dtos;
using Recipes.Application.Shared;
using Recipes.Domain;

namespace Recipes.Application.Features.Recipes;

/// <summary>
/// Service interface for recipe management operations.
/// </summary>
public interface IRecipeService
{
    /// <summary>
    /// Creates a new recipe.
    /// </summary>
    /// <param name="input">The recipe creation input.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing the created recipe's ID.</returns>
    Task<Result<Guid>> CreateAsync(CreateRecipeInput input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The recipe's unique identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing the recipe output.</returns>
    Task<Result<RecipeOutput>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a filtered and paginated list of recipes.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result containing a paginated list of recipes.</returns>
    Task<Result<PaginatedOutput<RecipeOutput>>> GetFilteredAsync(RecipeFilterInput filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    /// <param name="id">The recipe's unique identifier.</param>
    /// <param name="input">The recipe update input.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> UpdateAsync(Guid id, UpdateRecipeInput input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a recipe (soft delete).
    /// </summary>
    /// <param name="id">The recipe's unique identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a recipe as tried.
    /// </summary>
    /// <param name="id">The recipe's unique identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> MarkAsTriedAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a recipe as not tried.
    /// </summary>
    /// <param name="id">The recipe's unique identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> MarkAsNotTriedAsync(Guid id, CancellationToken cancellationToken = default);
}
