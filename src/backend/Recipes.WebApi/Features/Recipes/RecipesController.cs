using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Features.Recipes;
using Recipes.WebApi.Features.Recipes.Dtos;
using Recipes.WebApi.Shared;

namespace Recipes.WebApi.Features.Recipes;

/// <summary>
/// Controller for managing recipes.
/// </summary>
public class RecipesController(
    IRecipeService recipeService,
    ITagService tagService,
    IEquipmentService equipmentService) : ApiController
{
    /// <summary>
    /// Creates a new recipe.
    /// </summary>
    /// <param name="request">The recipe creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created recipe's ID.</returns>
    /// <response code="201">Returns the created recipe's ID.</response>
    /// <response code="400">If the request is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateRecipeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await recipeService.CreateAsync(request.ToInput(), cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Gets a recipe by ID.
    /// </summary>
    /// <param name="id">The recipe ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recipe.</returns>
    /// <response code="200">Returns the recipe.</response>
    /// <response code="404">If the recipe is not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await recipeService.GetByIdAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value!.ToResponse());
    }

    /// <summary>
    /// Gets a filtered and paginated list of recipes.
    /// </summary>
    /// <param name="request">The filter and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated list of recipes.</returns>
    /// <response code="200">Returns the list of recipes.</response>
    [HttpGet]
    [ProducesResponseType(typeof(RecipeListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<RecipeListResponse>> GetAll(
        [FromQuery] GetRecipesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await recipeService.GetFilteredAsync(request.ToInput(), cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value!.ToResponse());
    }

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    /// <param name="id">The recipe ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Recipe updated successfully.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the recipe is not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        Guid id,
        [FromBody] UpdateRecipeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await recipeService.UpdateAsync(id, request.ToInput(), cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound(result.Error);
            }

            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a recipe (soft delete).
    /// </summary>
    /// <param name="id">The recipe ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Recipe deleted successfully.</response>
    /// <response code="404">If the recipe is not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await recipeService.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Marks a recipe as tried.
    /// </summary>
    /// <param name="id">The recipe ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Recipe marked as tried.</response>
    /// <response code="404">If the recipe is not found.</response>
    [HttpPost("{id:guid}/tried")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> MarkAsTried(Guid id, CancellationToken cancellationToken)
    {
        var result = await recipeService.MarkAsTriedAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Marks a recipe as not tried.
    /// </summary>
    /// <param name="id">The recipe ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Recipe marked as not tried.</response>
    /// <response code="404">If the recipe is not found.</response>
    [HttpDelete("{id:guid}/tried")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> MarkAsNotTried(Guid id, CancellationToken cancellationToken)
    {
        var result = await recipeService.MarkAsNotTriedAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Gets all tags for autocomplete.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter tags.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of tags.</returns>
    /// <response code="200">Returns the list of tags.</response>
    [HttpGet("tags")]
    [ProducesResponseType(typeof(IEnumerable<TagResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TagResponse>>> GetTags(
        [FromQuery] string? searchTerm,
        CancellationToken cancellationToken)
    {
        var result = await tagService.GetAllAsync(searchTerm, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value!.Select(t => t.ToResponse()));
    }

    /// <summary>
    /// Gets all equipment for autocomplete.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter equipment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of equipment.</returns>
    /// <response code="200">Returns the list of equipment.</response>
    [HttpGet("equipment")]
    [ProducesResponseType(typeof(IEnumerable<EquipmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EquipmentResponse>>> GetEquipment(
        [FromQuery] string? searchTerm,
        CancellationToken cancellationToken)
    {
        var result = await equipmentService.GetAllAsync(searchTerm, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value!.Select(e => e.ToResponse()));
    }
}
