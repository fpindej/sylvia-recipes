using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Features.Recipes.Configurations;

internal class RecipeEquipmentConfiguration : IEntityTypeConfiguration<RecipeEquipment>
{
    public void Configure(EntityTypeBuilder<RecipeEquipment> builder)
    {
        builder.ToTable("recipe_equipment");

        // Composite primary key
        builder.HasKey(re => new { re.RecipeId, re.EquipmentId });

        builder.HasOne(re => re.Recipe)
            .WithMany(r => r.RecipeEquipment)
            .HasForeignKey(re => re.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(re => re.Equipment)
            .WithMany(e => e.RecipeEquipment)
            .HasForeignKey(re => re.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
