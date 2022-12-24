using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class PostConfiguration : IEntityTypeConfiguration<PostData>
{

    public void Configure(EntityTypeBuilder<PostData> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(p => p.Id);

        //builder
        //    .HasMany(p => p.Tags)
        //    .WithMany();
        builder
            .HasData(new PostData
            {
                Id = Guid.Parse("00000001-0000-0000-0000-000000000001"),
                Name = "TestPost",
                Content = "This is the content of the first test post.",
                AuthorId = Guid.Parse("00000003-0000-0000-0000-000000000002")
            });
        builder
            .HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity(j => j.HasData(new
            {
                PostsId = Guid.Parse("00000001-0000-0000-0000-000000000001"),
                TagsId = Guid.Parse("00000002-0000-0000-0000-000000000001")
            }));
    }
}
