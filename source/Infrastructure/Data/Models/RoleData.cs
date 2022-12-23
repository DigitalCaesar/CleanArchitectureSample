using Domain.Entities.Permissions;
using Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

[Table("Roles")]
public class RoleData
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<MemberData> Members { get; set; } = new List<MemberData>();
}
public sealed class RoleEnum : Enumeration<RoleEnum>
{
    public static readonly RoleEnum Registered = new(1, "Registered");

    public ICollection<Permission> Permissions { get; set; }
    public ICollection<MemberData> Members { get; set; }

    public RoleEnum(int id, string name)
        : base(id, name) { }


}
