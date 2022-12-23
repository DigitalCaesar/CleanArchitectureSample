using Data.Models;
using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class RoleEnumConfiguration : IEntityTypeConfiguration<RoleEnum>
{
    public void Configure(EntityTypeBuilder<RoleEnum> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();
        builder.HasMany(x => x.Members)
            .WithMany();

        builder.HasData(RoleEnum.GetValues());
    }
}
