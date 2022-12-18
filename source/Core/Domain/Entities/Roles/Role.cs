﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Roles;
public sealed class Role : BaseEntity
{
    /// <summary>
    /// The title of the Role
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// A description of the purpose or use
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Constructor requires values for all properties
    /// </summary>
    /// <param name="id">the unique identifier</param>
    /// <param name="name">the title of the tag</param>
    /// <param name="description">a description of the use of the role</param>
    private Role(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new object automatically adding an Id
    /// </summary>
    /// <param name="name">the title of the Role</param>
    /// <param name="description">a description of the Role</param>
    /// <returns>a Role</returns>
    public Role Create(string name, string description)
    {
        Guid id = Guid.NewGuid();
        return new Role(id, Name, description);
    }
    /// <summary>
    /// Creates a new object automatically adding an Id
    /// </summary>
    /// <param name="id">the unique identifier to index</param>
    /// <param name="name">the title of the Role</param>
    /// <param name="description">a description of the Role</param>
    /// <returns>a Role</returns>
    public Role Create(Guid id, string name, string description)
    {
        return new Role(id, Name, description);
    }
}
