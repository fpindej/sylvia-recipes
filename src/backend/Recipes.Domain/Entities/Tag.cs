using Recipes.Domain.Enums;

namespace Recipes.Domain.Entities;

/// <summary>
/// Represents a tag that can be associated with recipes for categorization.
/// </summary>
public class Tag : BaseEntity
{
    /// <summary>
    /// Gets the name of the tag.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the type of the tag.
    /// </summary>
    public TagType TagType { get; private set; }

    /// <summary>
    /// Gets the recipes associated with this tag.
    /// </summary>
    public ICollection<RecipeTag> RecipeTags { get; private set; } = [];

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Tag()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tag"/> class.
    /// </summary>
    /// <param name="name">The name of the tag.</param>
    /// <param name="tagType">The type of the tag.</param>
    public Tag(string name, TagType tagType)
    {
        Name = name;
        TagType = tagType;
    }
}
