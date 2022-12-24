using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class MemberConfiguration : IEntityTypeConfiguration<MemberData>
{

    public void Configure(EntityTypeBuilder<MemberData> builder)
    {
        builder.ToTable("Members");
        builder.HasKey(m => m.Id);
        //memberBuilder
        //    .HasMany(m => m.Roles)
        //    .WithMany();
        builder
            .HasData(new MemberData
            {
                Id = Guid.Parse("00000003-0000-0000-0000-000000000001"),
                Username = "User",
                Email = "user@test.com",
                FirstName = "First",
                LastName = "User"
            },
            new MemberData
            {
                Id = Guid.Parse("00000003-0000-0000-0000-000000000002"),
                Username = "Author",
                Email = "Author@test.com",
                FirstName = "Second",
                LastName = "User"
            },
            new MemberData
            {
                Id = Guid.Parse("00000003-0000-0000-0000-000000000003"),
                Username = "Admin",
                Email = "Admin@test.com",
                FirstName = "Third",
                LastName = "User"
            });
        builder
            .HasMany(p => p.Roles)
            .WithMany(t => t.Members)
            .UsingEntity(j => j.HasData(
                new
                {
                    MembersId = Guid.Parse("00000003-0000-0000-0000-000000000001"),
                    RolesId = Guid.Parse("00000004-0000-0000-0000-000000000001")
                },
                new
                {
                    MembersId = Guid.Parse("00000003-0000-0000-0000-000000000002"),
                    RolesId = Guid.Parse("00000004-0000-0000-0000-000000000002")
                },
                new
                {
                    MembersId = Guid.Parse("00000003-0000-0000-0000-000000000003"),
                    RolesId = Guid.Parse("00000004-0000-0000-0000-000000000003")
                }));
    }
}
