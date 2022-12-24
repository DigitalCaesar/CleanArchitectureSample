using Domain.ValueObjects;

namespace Domain.Entities.Tags;
public static class TagMapping
{
    public static Tag Map(this TagEntity entity)
    {
        Tag NewItem = new Tag()
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Description = entity.Description.Value
        };
        return NewItem;
    }

    public static TagEntity Map(this Tag data)
    {
        Name? Name = Name.Create(data.Name).Value;
        Description? Description = Description.Create(data.Description).Value;

        if (
            Name is null ||
            Description is null)
            throw new Domain.Exceptions.EntityMappingException("TagData");

        TagEntity NewItem = TagEntity.Create(
            data.Id,
            Name,
            Description);

        return NewItem;
    }
}
