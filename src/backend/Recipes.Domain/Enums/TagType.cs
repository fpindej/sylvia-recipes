namespace Recipes.Domain.Enums;

/// <summary>
/// Represents the type/category of a tag.
/// </summary>
public enum TagType
{
    /// <summary>
    /// Cuisine tag - e.g., taiwanese, japanese, italian, czech, french.
    /// </summary>
    Cuisine = 0,

    /// <summary>
    /// Type tag - e.g., soup, bread, pasta, salad, sweet, salty.
    /// </summary>
    Type = 1,

    /// <summary>
    /// Custom user-defined tag.
    /// </summary>
    Custom = 2
}
