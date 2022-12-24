using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class RoleEnumConfiguration : IEntityTypeConfiguration<RoleEnum>
{
    public void Configure(EntityTypeBuilder<RoleEnum> builder)
    {
        builder.ToTable("RoleEnums");
        builder.HasKey(x => x.Id);
        builder
            .HasMany(x => x.Permissions)
            .WithMany();
            //.UsingEntity<RolePermission>(
            //    rp => rp.HasData(
            //        new
            //        {
            //            RoleId = 1,
            //            RoleEnumId = 1,
            //            PermissionId = 1
            //        }));
        builder
            .HasMany(x => x.Members)
            .WithMany();

        var Roles = RoleEnum.GetValues();
        builder.HasData(Roles);
    }
}
