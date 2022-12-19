using Data.Models;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Tags;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data.Mapping;
internal static class PostMapping
{
    public static PostData Map(this Post entity)
    {
        List<TagData> Tags = entity.Tags.Select(x => (TagData)x.Map()).ToList();

        PostData NewItem = new PostData()
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Content = entity.Content.Value,
            AuthorId = entity.Author.Id, 
            Tags = Tags
        };
        return NewItem;
    }
    public static Post Map(this PostData data)
    {
        PostName? Name = PostName.Create(data.Name).Value;
        PostContent? Content = PostContent.Create(data.Content).Value;
        List<Tag> Tags = data.Tags.Select(x => (Tag)x.Map()).ToList();

        if (
            Name is null ||
            Content is null || 
            data.Author is null)
            throw new Data.Exceptions.InvalidDataException("PostData");

        Post NewItem = Post.Create(
            data.Id, 
            Name, 
            Content, 
            data.Author, 
            Tags);

        return NewItem;
    }
}
