using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tags;

/// <summary>
/// Categories added to objects
/// </summary>
public sealed class Tag : BaseEntity
{
    /// <summary>
    /// The title of the tag
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// A description of the tag
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Constructor requires values for all properties
    /// </summary>
    /// <param name="id">the unique identifier to index the tag</param>
    /// <param name="name">the title of the tag</param>
    /// <param name="description">a description of the tag</param>
    private Tag(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    /// <summary>
    /// Creates a new object automatically adding an Id
    /// </summary>
    /// <param name="name">the title of the tag</param>
    /// <param name="description">a description of the tag</param>
    /// <returns>a tag</returns>
    public static Tag Create(string name, string description)
    {
        var id = Guid.NewGuid();
        return new Tag(id, name, description);
    }
    /// <summary>
    /// Creates a new object from existing object data
    /// </summary>
    /// <param name="id">the unique identifier to index the tag</param>
    /// <param name="name">the title of the tag</param>
    /// <param name="description">a description of the tag</param>
    /// <returns>a tag</returns>
    public static Tag Create(Guid id, string name, string description)
    {
        return new Tag(id, name, description);
    }
}
