using JetBrains.Annotations;
using Recipes.Domain.Enums;

namespace Recipes.WebApi.Features.Recipes.Dtos;

/// <summary>
/// Response containing a single recipe.
/// </summary>
public class RecipeResponse
{
    /// <summary>
    /// The unique identifier of the recipe.
    /// </summary>
    public Guid Id { get; [UsedImplicitly] init; }

    /// <summary>
    /// The title of the recipe.
    /// </summary>
    public string Title { get; [UsedImplicitly] init; } = string.Empty;

    /// <summary>
    /// The step-by-step cooking instructions.
    /// </summary>
    public string Instructions { get; [UsedImplicitly] init; } = string.Empty;

    /// <summary>
    /// Description of the recipe.
    /// </summary>
    public string? Description { get; [UsedImplicitly] init; }

    /// <summary>
    /// Preparation time in minutes.
    /// </summary>
    public int? PrepTimeMinutes { get; [UsedImplicitly] init; }

    /// <summary>
    /// Cooking time in minutes.
    /// </summary>
    public int? CookTimeMinutes { get; [UsedImplicitly] init; }

    /// <summary>
    /// Number of servings.
    /// </summary>
    public int? Servings { get; [UsedImplicitly] init; }

    /// <summary>
    /// Protein content in grams.
    /// </summary>
    public decimal? ProteinGrams { get; [UsedImplicitly] init; }

    /// <summary>
    /// Whether the recipe has been tried.
    /// </summary>
    public bool IsTried { get; [UsedImplicitly] init; }

    /// <summary>
    /// URL of the original source.
    /// </summary>
    public string? SourceUrl { get; [UsedImplicitly] init; }

    /// <summary>
    /// URL of the recipe image.
    /// </summary>
    public string? ImageUrl { get; [UsedImplicitly] init; }

    /// <summary>
    /// Personal notes about the recipe.
    /// </summary>
    public string? Notes { get; [UsedImplicitly] init; }

    /// <summary>
    /// Workspace/counter space needed.
    /// </summary>
    public WorkspaceNeeded? WorkspaceNeeded { get; [UsedImplicitly] init; }

    /// <summary>
    /// Time category for the recipe.
    /// </summary>
    public TimeCategory? TimeCategory { get; [UsedImplicitly] init; }

    /// <summary>
    /// Messiness level of the recipe.
    /// </summary>
    public Messiness? Messiness { get; [UsedImplicitly] init; }

    /// <summary>
    /// Tags associated with the recipe.
    /// </summary>
    public List<TagResponse> Tags { get; [UsedImplicitly] init; } = [];

    /// <summary>
    /// Equipment needed for the recipe.
    /// </summary>
    public List<EquipmentResponse> Equipment { get; [UsedImplicitly] init; } = [];

    /// <summary>
    /// When the recipe was created.
    /// </summary>
    public DateTime CreatedAt { get; [UsedImplicitly] init; }

    /// <summary>
    /// When the recipe was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; [UsedImplicitly] init; }
}

/// <summary>
/// Response containing tag information.
/// </summary>
public class TagResponse
{
    /// <summary>
    /// The unique identifier of the tag.
    /// </summary>
    public Guid Id { get; [UsedImplicitly] init; }

    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name { get; [UsedImplicitly] init; } = string.Empty;

    /// <summary>
    /// The type of the tag.
    /// </summary>
    public TagType TagType { get; [UsedImplicitly] init; }
}

/// <summary>
/// Response containing equipment information.
/// </summary>
public class EquipmentResponse
{
    /// <summary>
    /// The unique identifier of the equipment.
    /// </summary>
    public Guid Id { get; [UsedImplicitly] init; }

    /// <summary>
    /// The name of the equipment.
    /// </summary>
    public string Name { get; [UsedImplicitly] init; } = string.Empty;
}
