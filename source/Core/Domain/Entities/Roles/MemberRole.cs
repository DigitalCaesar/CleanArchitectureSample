using Domain.Entities.Roles;

namespace Domain.Entities.Members;
public class MemberRole
{
    public Guid MemberId { get; set; }
    public int RoleId { get; set; }
}
