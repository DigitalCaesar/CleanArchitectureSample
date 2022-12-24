using Domain.Entities.Members;
using Domain.Entities.Permissions;
using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class RoleEnumConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);
        builder
            .HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>(rp => rp.HasData(
                Create(Role.Registered, Domain.Enums.Permission.ReadMember),
                Create(Role.Author, Domain.Enums.Permission.ReadMember),
                Create(Role.Author, Domain.Enums.Permission.WriteMember),
                Create(Role.Administrator, Domain.Enums.Permission.ReadMember),
                Create(Role.Administrator, Domain.Enums.Permission.WriteMember),
                Create(Role.Administrator, Domain.Enums.Permission.AdminMember)));
        builder
            .HasMany(x => x.Members)
            .WithMany();

        var Roles = Role.GetValues();
        builder.HasData(Roles);

    }
    private static RolePermission Create(Role role, Domain.Enums.Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}
