using Recipes.Domain.Enums;

namespace Recipes.Application.Features.Recipes.Dtos;

/// <summary>
/// Input data for filtering recipes.
/// </summary>
public record RecipeFilterInput(
    string? SearchTerm = null,
    bool? IsTried = null,
    IReadOnlyList<string>? Cuisines = null,
    IReadOnlyList<string>? Types = null,
    IReadOnlyList<string>? EquipmentNames = null,
    WorkspaceNeeded? WorkspaceNeeded = null,
    TimeCategory? TimeCategory = null,
    Messiness? Messiness = null,
    decimal? MinProteinGrams = null,
    int PageNumber = 1,
    int PageSize = 10
);
