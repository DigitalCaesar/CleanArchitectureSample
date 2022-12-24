using Data.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{

    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(t => new { t.RoleId, t.PermissionId });
        builder.HasData(
            Create(RoleEnum.Registered, Permission.ReadMember));
    }
    private static RolePermission Create(RoleEnum role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}
