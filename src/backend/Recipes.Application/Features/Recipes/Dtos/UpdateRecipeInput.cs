using Recipes.Domain.Enums;

namespace Recipes.Application.Features.Recipes.Dtos;

/// <summary>
/// Input data for updating an existing recipe. All fields are optional for partial updates.
/// </summary>
public record UpdateRecipeInput(
    string? Title = null,
    string? Instructions = null,
    string? Description = null,
    int? PrepTimeMinutes = null,
    int? CookTimeMinutes = null,
    int? Servings = null,
    decimal? ProteinGrams = null,
    bool? IsTried = null,
    string? SourceUrl = null,
    string? ImageUrl = null,
    string? Notes = null,
    WorkspaceNeeded? WorkspaceNeeded = null,
    TimeCategory? TimeCategory = null,
    Messiness? Messiness = null,
    IReadOnlyList<TagInput>? Tags = null,
    IReadOnlyList<string>? EquipmentNames = null
);
