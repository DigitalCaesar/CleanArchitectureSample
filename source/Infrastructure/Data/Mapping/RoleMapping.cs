using Data.Models;
using Domain.Entities.Roles;
using Domain.ValueObjects;

namespace Data.Mapping;
internal static class RoleMapping
{
    public static RoleData Map(this Role entity)
    {
        RoleData NewItem = new RoleData()
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Description = entity.Description.Value
        };
        return NewItem;
    }

    public static Role Map(this RoleData data)
    {
        Name? Name = Name.Create(data.Name).Value;
        Description? Description = Description.Create(data.Description).Value;

        if (
            Name is null ||
            Description is null)
            throw new Data.Exceptions.InvalidDataException("RoleData");

        Role NewItem = Role.Create(
            data.Id,
            Name,
            Description);

        return NewItem;
    }
}
