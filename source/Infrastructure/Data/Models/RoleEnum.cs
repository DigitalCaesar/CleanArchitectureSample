using Domain.Entities.Permissions;
using Domain.Shared;

namespace Data.Models;

public sealed class RoleEnum : Enumeration<RoleEnum>
{
    public static readonly RoleEnum Registered = new(1, "Registered");

    public ICollection<Permission> Permissions { get; set; }
    public ICollection<MemberData> Members { get; set; }

    public RoleEnum(int id, string name)
        : base(id, name) { }


}
