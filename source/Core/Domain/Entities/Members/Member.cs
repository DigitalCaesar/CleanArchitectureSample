using Domain.Entities.Roles;
using Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Members;

/// <summary>
/// An account for a person with access to the system
/// </summary>
public sealed class Member : Entity
{
    private readonly List<Role> mRoles = new();

    /// <summary>
    /// The unique user name of the member
    /// </summary>
    public string Username { get; private set; }
    /// <summary>
    /// The email address of the member
    /// </summary>
    public string Email { get; private set; }
    /// <summary>
    /// The first name of the member
    /// </summary>
    public string FirstName { get; private set; }
    /// <summary>
    /// The last name of the member
    /// </summary>
    public string LastName { get; private set; }
    /// <summary>
    /// The full name of the member
    /// </summary>
    public string FullName { get { return string.Concat(FirstName, LastName); } }
    /// <summary>
    /// A list of Roles assigned to the member
    /// </summary>
    public ReadOnlyCollection<Role> Roles => mRoles.AsReadOnly();


    /// <summary>
    /// Constructor requires values for all properties
    /// </summary>
    /// <param name="id">the unique identifier to index the Member</param>
    /// <param name="username">the unique user name of the member</param>
    /// <param name="email">the email address of the member</param>
    /// <param name="firstName">the first name of the user</param>
    /// <param name="lastName">the last name of the user</param>
    /// <param name="roles">a list of roles assigned to the member</param>
    private Member(Guid id, string username, string email, string firstName, string lastName, List<Role> roles) 
        : base(id)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        mRoles = roles;
    }
    /// <summary>
    /// Creates a new member automatically adding an Id
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="roles">a list of roles assigned to the member</param>
    /// <returns>a member</returns>
    public static Member Create(string username, string email, string firstName, string lastName, List<Role> roles)
    {
        var id = Guid.NewGuid();
        return new Member(id, username, email, firstName, lastName, roles);
    }
    /// <summary>
    /// Creates a new member from existing member data
    /// </summary>
    /// <param name="id">the unique identifier to index the Member</param>
    /// <param name="username">the unique user name of the member</param>
    /// <param name="email">the email address of the member</param>
    /// <param name="firstName">the first name of the user</param>
    /// <param name="lastName">the last name of the user</param>
    /// <param name="roles">a list of roles assigned to the member</param>
    /// <returns>a member</returns>
    public static Member Create(Guid id, string username, string email, string firstName, string lastName, List<Role> roles)
    {
        return new Member(id, username, email, firstName, lastName, roles);
    }

    /// <summary>
    /// Enables a new Role for the member
    /// </summary>
    /// <param name="role">a role to assign to the member</param>
    public void AddRole(Role role)
    {
        var ExistingItem = mRoles.Find(x => x.Name == role.Name);
        if (ExistingItem is null)
            mRoles.Add(role);
    }
    /// <summary>
    /// Disables an existing Role for the member
    /// </summary>
    /// <param name="role">a role to remove from the member</param>
    public void RemoveRole(Role role)
    {
        var ExistingItem = mRoles.Find(x => x.Name == role.Name);
        if (ExistingItem is not null)
            mRoles.Remove(role);
    }
}

