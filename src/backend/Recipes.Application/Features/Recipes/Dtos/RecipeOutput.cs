using Recipes.Domain.Enums;

namespace Recipes.Application.Features.Recipes.Dtos;

/// <summary>
/// Output data for a recipe.
/// </summary>
public record RecipeOutput(
    Guid Id,
    string Title,
    string Instructions,
    string? Description,
    int? PrepTimeMinutes,
    int? CookTimeMinutes,
    int? Servings,
    decimal? ProteinGrams,
    bool IsTried,
    string? SourceUrl,
    string? ImageUrl,
    string? Notes,
    WorkspaceNeeded? WorkspaceNeeded,
    TimeCategory? TimeCategory,
    Messiness? Messiness,
    IReadOnlyList<TagOutput> Tags,
    IReadOnlyList<EquipmentOutput> Equipment,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

/// <summary>
/// Output data for a tag.
/// </summary>
public record TagOutput(Guid Id, string Name, TagType TagType);

/// <summary>
/// Output data for equipment.
/// </summary>
public record EquipmentOutput(Guid Id, string Name);
