using Domain.Entities.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;
internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(x => x.Id);

        IEnumerable<Permission> permissions = Enum.GetValues<Domain.Enums.Permission>()
            .Select(p => new Permission
            {
                Id = (int)p,
                Name = p.ToString()
            });


        builder.HasData(permissions);
    }
}
