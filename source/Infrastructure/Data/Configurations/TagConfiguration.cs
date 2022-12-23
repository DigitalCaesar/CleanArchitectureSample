using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class TagConfiguration : IEntityTypeConfiguration<TagData>
{

    public void Configure(EntityTypeBuilder<TagData> builder)
    {
        builder
            .HasKey(t => t.Id);
        builder
            .HasData(new TagData
            {
                Id = Guid.Parse("00000002-0000-0000-0000-000000000001"),
                Name = "Test",
                Description = "Test Category"
            });
    }
}
