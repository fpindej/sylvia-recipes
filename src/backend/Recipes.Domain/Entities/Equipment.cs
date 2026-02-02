namespace Recipes.Domain.Entities;

/// <summary>
/// Represents a piece of equipment that can be needed for recipes.
/// </summary>
public class Equipment : BaseEntity
{
    /// <summary>
    /// Gets the name of the equipment.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the recipes that require this equipment.
    /// </summary>
    public ICollection<RecipeEquipment> RecipeEquipment { get; private set; } = [];

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Equipment()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Equipment"/> class.
    /// </summary>
    /// <param name="name">The name of the equipment.</param>
    public Equipment(string name)
    {
        Name = name;
    }
}
