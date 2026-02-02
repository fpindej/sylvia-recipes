namespace Recipes.Domain.Entities;

/// <summary>
/// Represents the many-to-many relationship between Recipe and Equipment.
/// </summary>
public class RecipeEquipment
{
    /// <summary>
    /// Gets the recipe identifier.
    /// </summary>
    public Guid RecipeId { get; private set; }

    /// <summary>
    /// Gets the equipment identifier.
    /// </summary>
    public Guid EquipmentId { get; private set; }

    /// <summary>
    /// Gets the associated recipe.
    /// </summary>
    public Recipe Recipe { get; private set; } = null!;

    /// <summary>
    /// Gets the associated equipment.
    /// </summary>
    public Equipment Equipment { get; private set; } = null!;

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected RecipeEquipment()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeEquipment"/> class.
    /// </summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <param name="equipmentId">The equipment identifier.</param>
    public RecipeEquipment(Guid recipeId, Guid equipmentId)
    {
        RecipeId = recipeId;
        EquipmentId = equipmentId;
    }
}
