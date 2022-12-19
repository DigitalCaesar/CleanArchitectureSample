using Data.Models;
using Domain.Entities.Tags;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping;
internal static class TagMapping
{
    public static TagData Map(this Tag entity)
    {
        TagData NewItem = new TagData()
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Description = entity.Description.Value
        };
        return NewItem;
    }

    public static Tag Map(this TagData data)
    {
        Name? Name = Name.Create(data.Name).Value;
        Description? Description = Description.Create(data.Description).Value;

        if (
            Name is null ||
            Description is null)
            throw new Data.Exceptions.InvalidDataException("TagData");

        Tag NewItem = Tag.Create(
            data.Id,
            Name,
            Description);

        return NewItem;
    }
}
