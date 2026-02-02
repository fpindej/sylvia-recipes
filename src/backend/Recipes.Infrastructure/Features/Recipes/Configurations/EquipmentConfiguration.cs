using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Persistence.Configurations;

namespace Recipes.Infrastructure.Features.Recipes.Configurations;

internal class EquipmentConfiguration : BaseEntityConfiguration<Equipment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("equipment");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Unique constraint on Name
        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}
