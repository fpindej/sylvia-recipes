using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Persistence.Configurations;

namespace Recipes.Infrastructure.Features.Recipes.Configurations;

internal class TagConfiguration : BaseEntityConfiguration<Tag>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.TagType)
            .IsRequired();

        // Unique constraint on Name + TagType combination
        builder.HasIndex(t => new { t.Name, t.TagType })
            .IsUnique();

        // Index for searching tags by name
        builder.HasIndex(t => t.Name);
    }
}
