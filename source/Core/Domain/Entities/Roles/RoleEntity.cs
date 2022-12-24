using Domain.Entities.Members;
using Domain.Entities.Permissions;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities.Roles;
public sealed class RoleEntity : Entity
{
    /// <summary>
    /// The title of the Role
    /// </summary>
    public Name Name { get; private set; }
    /// <summary>
    /// A description of the purpose or use
    /// </summary>
    public Description Description { get; private set; }

    /// <summary>
    /// Constructor requires values for all properties
    /// </summary>
    /// <param name="id">the unique identifier</param>
    /// <param name="name">the title of the tag</param>
    /// <param name="description">a description of the use of the role</param>
    private RoleEntity(Guid id, Name name, Description description)
        : base(id)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new object automatically adding an Id
    /// </summary>
    /// <param name="name">the title of the Role</param>
    /// <param name="description">a description of the Role</param>
    /// <returns>a Role</returns>
    public static RoleEntity Create(Name name, Description description)
    {
        Guid id = Guid.NewGuid();
        return new RoleEntity(id, name, description);
    }
    /// <summary>
    /// Creates a new object automatically adding an Id
    /// </summary>
    /// <param name="id">the unique identifier to index</param>
    /// <param name="name">the title of the Role</param>
    /// <param name="description">a description of the Role</param>
    /// <returns>a Role</returns>
    public static RoleEntity Create(Guid id, Name name, Description description)
    {
        return new RoleEntity(id, name, description);
    }
}