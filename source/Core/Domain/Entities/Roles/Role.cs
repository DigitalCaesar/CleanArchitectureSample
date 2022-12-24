using Domain.Entities.Members;
using Domain.Entities.Permissions;
using Domain.Shared;

namespace Domain.Entities.Roles;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Registered = new(1, "Registered");
    public static readonly Role Author = new(2, "Author");
    public static readonly Role Administrator = new(3, "Administrator");

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public ICollection<Member> Members { get; set; } = new List<Member>();

    public Role(int id, string name)
        : base(id, name) { }


}
