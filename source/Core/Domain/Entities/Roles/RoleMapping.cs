using Domain.ValueObjects;

namespace Domain.Entities.Roles;
internal static class RoleMapping
{
    public static Role Map(this RoleEntity entity)
    {
        Role NewItem = new Role()
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Description = entity.Description.Value
        };
        return NewItem;
    }

    public static RoleEntity Map(this Role data)
    {
        Name? Name = Name.Create(data.Name).Value;
        Description? Description = Description.Create(data.Description).Value;

        if (
            Name is null ||
            Description is null)
            throw new Domain.Exceptions.EntityMappingException("RoleData");

        RoleEntity NewItem = RoleEntity.Create(
            data.Id,
            Name,
            Description);

        return NewItem;
    }
}
