using Domain.Entities.Members;
using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Runtime.InteropServices;

namespace Data.Configurations;
internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{

    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");
        builder.HasKey(m => m.Id);
        //builder
        //    .HasMany(x => x.Roles)
        //    .WithMany();
        builder
            .HasData(
                Create(
                    "00000003-0000-0000-0000-000000000001",
                    "User",
                    "user@test.com",
                    "First",
                    "User"),
                Create(
                    "00000003-0000-0000-0000-000000000002",
                    "Author",
                    "Author@test.com",
                    "Second",
                    "User"),
                Create(
                    "00000003-0000-0000-0000-000000000003",
                    "Admin",
                    "Admin@test.com",
                    "Third",
                    "User"));
        builder
            .HasMany(p => p.Roles)
            .WithMany()
            .UsingEntity<MemberRole>(j => j.HasData(
                Create("00000003-0000-0000-0000-000000000001", Role.Registered),
                Create("00000003-0000-0000-0000-000000000002", Role.Author),
                Create("00000003-0000-0000-0000-000000000003", Role.Administrator)
            ));
    }
    private Member Create(string id, string username, string email, string firstname, string lastname)
    {
        return new Member
        {
            Id = Guid.Parse(id),
            Username = username,
            Email = email,
            FirstName = firstname,
            LastName = lastname
        };
    }
    private static MemberRole Create(string memberId, Role role)
    {
        return new MemberRole
        {
            MemberId = Guid.Parse(memberId),
            RoleId = role.Id
        };
    }
}
