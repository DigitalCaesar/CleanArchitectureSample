using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain;
public abstract class Entity : IEquatable<Entity>
{
    /// <summary>
    /// A unique identifer for the Entity
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Constructor requires all properties to be initialized
    /// </summary>
    /// <param name="id"></param>
    protected Entity(Guid id) 
    {
        Id = id;
    }

    
    public override bool Equals(object? obj)
    {
        // Check if null
        if(obj is null)
        {
            return false;
        }

        // Check for incompatible types
        if(obj.GetType() != GetType())
        {
            return false;
        }

        // Check if not same type 
        if(obj is not Entity entity)
        {
            return false;
        }

        // Finally, compare the two objects
        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }

    public bool Equals(Entity? other)
    {
        // Check if null
        if (other is null)
        {
            return false;
        }

        // Check for incompatible types
        if (other.GetType() != GetType())
        {
            return false;
        }

        // Finally, compare the two objects
        return other.Id == Id;
    }
    public static bool operator ==(Entity? left, Entity? right)
    {
        return (left is not null && right is not null && left.Equals(right));
    }
    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}
