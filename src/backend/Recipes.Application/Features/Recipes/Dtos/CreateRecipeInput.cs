using Recipes.Domain.Enums;

namespace Recipes.Application.Features.Recipes.Dtos;

/// <summary>
/// Input data for creating a new recipe.
/// </summary>
public record CreateRecipeInput(
    string Title,
    string Instructions,
    string? Description = null,
    int? PrepTimeMinutes = null,
    int? CookTimeMinutes = null,
    int? Servings = null,
    decimal? ProteinGrams = null,
    bool IsTried = false,
    string? SourceUrl = null,
    string? ImageUrl = null,
    string? Notes = null,
    WorkspaceNeeded? WorkspaceNeeded = null,
    TimeCategory? TimeCategory = null,
    Messiness? Messiness = null,
    IReadOnlyList<TagInput>? Tags = null,
    IReadOnlyList<string>? EquipmentNames = null
);

/// <summary>
/// Input for creating or referencing a tag.
/// </summary>
/// <param name="Name">The name of the tag.</param>
/// <param name="TagType">The type of the tag.</param>
public record TagInput(string Name, TagType TagType);
