//using Domain.Entities.Members;
//using Domain.Entities.Roles;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Data.Configurations;
//internal sealed class MemberRoleConfiguration : IEntityTypeConfiguration<MemberRole>
//{

//    public void Configure(EntityTypeBuilder<MemberRole> builder)
//    {
//        builder.HasKey(t => new { t.MemberId, t.RoleId });
//        builder
//            .HasData(
//            Create(
//                "00000003-0000-0000-0000-000000000001",
//                Role.Registered),
//            Create(
//                "00000003-0000-0000-0000-000000000002",
//                Role.Author),
//            Create(
//                "00000003-0000-0000-0000-000000000003",
//                Role.Administrator)
//            ) ;
//    }
//    private static MemberRole Create(string memberId, Role role)
//    {
//        return new MemberRole
//        {
//            MemberId = Guid.Parse(memberId),
//            RoleId = role.Id
//        };
//    }
//}
