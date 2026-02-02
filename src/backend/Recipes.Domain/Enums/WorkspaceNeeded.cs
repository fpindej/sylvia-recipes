namespace Recipes.Domain.Enums;

/// <summary>
/// Represents the amount of workspace/counter space needed for preparing a recipe.
/// </summary>
public enum WorkspaceNeeded
{
    /// <summary>
    /// Small workspace - just a small chopping board.
    /// </summary>
    Small = 0,

    /// <summary>
    /// Medium workspace - moderate counter space.
    /// </summary>
    Medium = 1,

    /// <summary>
    /// Large workspace - whole worktop needed.
    /// </summary>
    Large = 2
}
