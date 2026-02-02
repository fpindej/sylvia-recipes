using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Persistence.Configurations;

namespace Recipes.Infrastructure.Features.Recipes.Configurations;

internal class RecipeConfiguration : BaseEntityConfiguration<Recipe>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipes");

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.Description)
            .HasMaxLength(2000);

        builder.Property(r => r.Instructions)
            .IsRequired();

        builder.Property(r => r.PrepTimeMinutes);

        builder.Property(r => r.CookTimeMinutes);

        builder.Property(r => r.Servings);

        builder.Property(r => r.ProteinGrams)
            .HasPrecision(10, 2);

        builder.Property(r => r.IsTried)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.SourceUrl)
            .HasMaxLength(2048);

        builder.Property(r => r.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(r => r.Notes);

        builder.Property(r => r.WorkspaceNeeded);

        builder.Property(r => r.TimeCategory);

        builder.Property(r => r.Messiness);

        // Indexes for common queries
        builder.HasIndex(r => r.IsTried);
        builder.HasIndex(r => r.TimeCategory);
        builder.HasIndex(r => r.WorkspaceNeeded);

        // GIN index for full-text search on title and description
        // This requires pg_trgm extension
        builder.HasIndex(r => r.Title)
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops");

        builder.HasIndex(r => r.Description)
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops");
    }
}
