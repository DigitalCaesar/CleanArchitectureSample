using Domain.Entities.Roles;

namespace Domain.Entities.Members;

public class Member
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public Member() { }
}


