namespace Recipes.Domain.Entities;

/// <summary>
/// Represents the many-to-many relationship between Recipe and Tag.
/// </summary>
public class RecipeTag
{
    /// <summary>
    /// Gets the recipe identifier.
    /// </summary>
    public Guid RecipeId { get; private set; }

    /// <summary>
    /// Gets the tag identifier.
    /// </summary>
    public Guid TagId { get; private set; }

    /// <summary>
    /// Gets the associated recipe.
    /// </summary>
    public Recipe Recipe { get; private set; } = null!;

    /// <summary>
    /// Gets the associated tag.
    /// </summary>
    public Tag Tag { get; private set; } = null!;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected RecipeTag()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeTag"/> class.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="tagId">The tag identifier.</param>
    public RecipeTag(Guid recipeId, Guid tagId)
    {
        RecipeId = recipeId;
        TagId = tagId;
    }
}
