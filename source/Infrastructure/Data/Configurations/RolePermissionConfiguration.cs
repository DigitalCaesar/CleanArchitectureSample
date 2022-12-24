//using Domain.Entities.Permissions;
//using Domain.Entities.Roles;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Data.Configurations;
//internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
//{

//    public void Configure(EntityTypeBuilder<RolePermission> builder)
//    {
//        builder.HasKey(t => new { t.RoleId, t.PermissionId });
//        builder.HasData(
//            Create(Role.Registered, Domain.Enums.Permission.ReadMember));
//    }
//    private static RolePermission Create(Role role, Domain.Enums.Permission permission)
//    {
//        return new RolePermission
//        {
//            RoleId = role.Id,
//            PermissionId = (int)permission
//        };
//    }
//}
