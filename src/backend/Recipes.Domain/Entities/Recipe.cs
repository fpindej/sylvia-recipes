using Recipes.Domain.Enums;

namespace Recipes.Domain.Entities;

/// <summary>
/// Represents a recipe in the recipe collection.
/// </summary>
public class Recipe : BaseEntity
{
    /// <summary>
    /// Gets the title of the recipe.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the optional description of the recipe.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the step-by-step cooking instructions.
    /// </summary>
    public string Instructions { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the preparation time in minutes.
    /// </summary>
    public int? PrepTimeMinutes { get; private set; }

    /// <summary>
    /// Gets the cooking time in minutes.
    /// </summary>
    public int? CookTimeMinutes { get; private set; }

    /// <summary>
    /// Gets the number of servings this recipe yields.
    /// </summary>
    public int? Servings { get; private set; }

    /// <summary>
    /// Gets the protein content in grams (per serving or total, depending on context).
    /// </summary>
    public decimal? ProteinGrams { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the user has tried this recipe.
    /// </summary>
    public bool IsTried { get; private set; }

    /// <summary>
    /// Gets the original source URL where the recipe was found.
    /// </summary>
    public string? SourceUrl { get; private set; }

    /// <summary>
    /// Gets the URL of the recipe image.
    /// </summary>
    public string? ImageUrl { get; private set; }

    /// <summary>
    /// Gets personal notes and tweaks for the recipe.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets the amount of workspace/counter space needed.
    /// </summary>
    public WorkspaceNeeded? WorkspaceNeeded { get; private set; }

    /// <summary>
    /// Gets the time category for the recipe.
    /// </summary>
    public TimeCategory? TimeCategory { get; private set; }

    /// <summary>
    /// Gets the messiness level of the recipe.
    /// </summary>
    public Messiness? Messiness { get; private set; }

    /// <summary>
    /// Gets the tags associated with this recipe.
    /// </summary>
    public ICollection<RecipeTag> RecipeTags { get; private set; } = [];

    /// <summary>
    /// Gets the equipment needed for this recipe.
    /// </summary>
    public ICollection<RecipeEquipment> RecipeEquipment { get; private set; } = [];

    /// <summary>
    /// Protected constructor for EF Core.
    /// </summary>
    protected Recipe()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Recipe"/> class.
    /// </summary>
    /// <param name="title">The title of the recipe.</param>
    /// <param name="instructions">The step-by-step cooking instructions.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="prepTimeMinutes">Optional preparation time in minutes.</param>
    /// <param name="cookTimeMinutes">Optional cooking time in minutes.</param>
    /// <param name="servings">Optional number of servings.</param>
    /// <param name="proteinGrams">Optional protein content in grams.</param>
    /// <param name="isTried">Whether the recipe has been tried.</param>
    /// <param name="sourceUrl">Optional source URL.</param>
    /// <param name="imageUrl">Optional image URL.</param>
    /// <param name="notes">Optional personal notes.</param>
    /// <param name="workspaceNeeded">Optional workspace requirement.</param>
    /// <param name="timeCategory">Optional time category.</param>
    /// <param name="messiness">Optional messiness level.</param>
    public Recipe(
        string title,
        string instructions,
        string? description = null,
        int? prepTimeMinutes = null,
        int? cookTimeMinutes = null,
        int? servings = null,
        decimal? proteinGrams = null,
        bool isTried = false,
        string? sourceUrl = null,
        string? imageUrl = null,
        string? notes = null,
        WorkspaceNeeded? workspaceNeeded = null,
        TimeCategory? timeCategory = null,
        Messiness? messiness = null)
    {
        Title = title;
        Instructions = instructions;
        Description = description;
        PrepTimeMinutes = prepTimeMinutes;
        CookTimeMinutes = cookTimeMinutes;
        Servings = servings;
        ProteinGrams = proteinGrams;
        IsTried = isTried;
        SourceUrl = sourceUrl;
        ImageUrl = imageUrl;
        Notes = notes;
        WorkspaceNeeded = workspaceNeeded;
        TimeCategory = timeCategory;
        Messiness = messiness;
    }

    /// <summary>
    /// Updates the recipe with new values.
    /// </summary>
    public void Update(
        string? title = null,
        string? instructions = null,
        string? description = null,
        int? prepTimeMinutes = null,
        int? cookTimeMinutes = null,
        int? servings = null,
        decimal? proteinGrams = null,
        bool? isTried = null,
        string? sourceUrl = null,
        string? imageUrl = null,
        string? notes = null,
        WorkspaceNeeded? workspaceNeeded = null,
        TimeCategory? timeCategory = null,
        Messiness? messiness = null)
    {
        if (title is not null) Title = title;
        if (instructions is not null) Instructions = instructions;
        if (description is not null) Description = description;
        if (prepTimeMinutes.HasValue) PrepTimeMinutes = prepTimeMinutes;
        if (cookTimeMinutes.HasValue) CookTimeMinutes = cookTimeMinutes;
        if (servings.HasValue) Servings = servings;
        if (proteinGrams.HasValue) ProteinGrams = proteinGrams;
        if (isTried.HasValue) IsTried = isTried.Value;
        if (sourceUrl is not null) SourceUrl = sourceUrl;
        if (imageUrl is not null) ImageUrl = imageUrl;
        if (notes is not null) Notes = notes;
        if (workspaceNeeded.HasValue) WorkspaceNeeded = workspaceNeeded;
        if (timeCategory.HasValue) TimeCategory = timeCategory;
        if (messiness.HasValue) Messiness = messiness;
    }

    /// <summary>
    /// Marks the recipe as tried.
    /// </summary>
    public void MarkAsTried() => IsTried = true;

    /// <summary>
    /// Marks the recipe as not tried.
    /// </summary>
    public void MarkAsNotTried() => IsTried = false;
}
