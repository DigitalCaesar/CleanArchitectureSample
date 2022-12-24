using Domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{

    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        builder.HasKey(t => t.Id);
        builder
            .HasData(new Tag
            {
                Id = Guid.Parse("00000002-0000-0000-0000-000000000001"),
                Name = "Test",
                Description = "Test Category"
            });
    }
}
