using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Recipes.Domain.Enums;

namespace Recipes.WebApi.Features.Recipes.Dtos;

/// <summary>
/// Request to update an existing recipe. All fields are optional for partial updates.
/// </summary>
public class UpdateRecipeRequest
{
    /// <summary>
    /// The title of the recipe.
    /// </summary>
    [MaxLength(255)]
    public string? Title { get; [UsedImplicitly] set; }

    /// <summary>
    /// The step-by-step cooking instructions.
    /// </summary>
    public string? Instructions { get; [UsedImplicitly] set; }

    /// <summary>
    /// Description of the recipe.
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; [UsedImplicitly] set; }

    /// <summary>
    /// Preparation time in minutes.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? PrepTimeMinutes { get; [UsedImplicitly] set; }

    /// <summary>
    /// Cooking time in minutes.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? CookTimeMinutes { get; [UsedImplicitly] set; }

    /// <summary>
    /// Number of servings.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? Servings { get; [UsedImplicitly] set; }

    /// <summary>
    /// Protein content in grams.
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal? ProteinGrams { get; [UsedImplicitly] set; }

    /// <summary>
    /// Whether the recipe has been tried.
    /// </summary>
    public bool? IsTried { get; [UsedImplicitly] set; }

    /// <summary>
    /// URL of the original source.
    /// </summary>
    [MaxLength(2048)]
    [Url]
    public string? SourceUrl { get; [UsedImplicitly] set; }

    /// <summary>
    /// URL of the recipe image.
    /// </summary>
    [MaxLength(2048)]
    [Url]
    public string? ImageUrl { get; [UsedImplicitly] set; }

    /// <summary>
    /// Personal notes about the recipe.
    /// </summary>
    public string? Notes { get; [UsedImplicitly] set; }

    /// <summary>
    /// Workspace/counter space needed.
    /// </summary>
    public WorkspaceNeeded? WorkspaceNeeded { get; [UsedImplicitly] set; }

    /// <summary>
    /// Time category for the recipe.
    /// </summary>
    public TimeCategory? TimeCategory { get; [UsedImplicitly] set; }

    /// <summary>
    /// Messiness level of the recipe.
    /// </summary>
    public Messiness? Messiness { get; [UsedImplicitly] set; }

    /// <summary>
    /// Tags to associate with the recipe (replaces existing tags).
    /// </summary>
    public List<TagRequest>? Tags { get; [UsedImplicitly] set; }

    /// <summary>
    /// Equipment names needed for the recipe (replaces existing equipment).
    /// </summary>
    public List<string>? EquipmentNames { get; [UsedImplicitly] set; }
}
