namespace Recipes.Domain.Enums;

/// <summary>
/// Represents the messiness level of preparing a recipe.
/// </summary>
public enum Messiness
{
    /// <summary>
    /// Low messiness - minimal cleanup required.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium messiness - moderate cleanup required.
    /// </summary>
    Medium = 1,

    /// <summary>
    /// High messiness - lots of chopping, flour everywhere, significant cleanup.
    /// </summary>
    High = 2
}
