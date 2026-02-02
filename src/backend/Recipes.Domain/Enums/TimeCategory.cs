namespace Recipes.Domain.Enums;

/// <summary>
/// Represents the time category for preparing and cooking a recipe.
/// </summary>
public enum TimeCategory
{
    /// <summary>
    /// Quick recipe - 30 minutes or less, suitable for lunch hour.
    /// </summary>
    Quick = 0,

    /// <summary>
    /// Medium time - 1-3 hours, needs some planning.
    /// </summary>
    Medium = 1,

    /// <summary>
    /// Long recipe - more than 3 hours.
    /// </summary>
    Long = 2,

    /// <summary>
    /// Overnight or longer - requires significant time (e.g., marinating, slow cooking).
    /// </summary>
    Overnight = 3
}
