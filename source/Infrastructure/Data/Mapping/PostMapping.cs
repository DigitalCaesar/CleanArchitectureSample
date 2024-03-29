﻿using Domain.Entities.Posts;
using Domain.Entities.Tags;
using Domain.ValueObjects;

namespace Data.Mapping;
internal static class PostMapping
{
    public static Post Map(this PostEntity entity)
    {
        //MemberData Author = (MemberData)entity.Author.Map();
        List<Tag> Tags = entity.Tags.Select(x => (Tag)x.Map()).ToList();
        //List<Tag> Tags = entity.Tags.Select(x => new PostTag() 
        //{ 
        //    PostId = entity.Id, 
        //    TagId = x.Id 
        //}).ToList();

        Post NewItem = new Post()
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Content = entity.Content.Value,
            AuthorId = entity.AuthorId, 
            //Author = Author,
            Tags = Tags
        };
        return NewItem;
    }
    public static PostEntity Map(this Post data)
    {
        if (data is null)
            return default!;

        PostName? Name = PostName.Create(data.Name).Value;
        PostContent? Content = PostContent.Create(data.Content).Value;
        List<TagEntity> Tags = data.Tags.Select(x => x.Map()).ToList();
        //List<Tag> Tags = data.Tags.Select(x => Tag.Create(
        //    x.TagId, 
        //    Domain.ValueObjects.Name.Create(x.Tag.Name).Value, 
        //    Description.Create(x.Tag.Description).Value
        //    )).ToList();

        if (
            Name is null ||
            Content is null)
            throw new Data.Exceptions.InvalidDataException("PostData");

        //Member Author = data.Author.Map();

        PostEntity NewItem = PostEntity.Create(
            data.Id, 
            Name, 
            Content, 
            data.AuthorId, 
            Tags);

        return NewItem;
    }
}
