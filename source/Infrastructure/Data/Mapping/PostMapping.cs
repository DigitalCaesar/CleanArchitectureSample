using Data.Models;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Tags;
using Domain.ValueObjects;

namespace Data.Mapping;
internal static class PostMapping
{
    public static PostData Map(this Post entity)
    {
        //MemberData Author = (MemberData)entity.Author.Map();
        List<TagData> Tags = entity.Tags.Select(x => (TagData)x.Map()).ToList();
        //List<Tag> Tags = entity.Tags.Select(x => new PostTag() 
        //{ 
        //    PostId = entity.Id, 
        //    TagId = x.Id 
        //}).ToList();

        PostData NewItem = new PostData()
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
    public static Post Map(this PostData data)
    {
        if (data is null)
            return default!;

        PostName? Name = PostName.Create(data.Name).Value;
        PostContent? Content = PostContent.Create(data.Content).Value;
        List<Tag> Tags = data.Tags.Select(x => x.Map()).ToList();
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

        Post NewItem = Post.Create(
            data.Id, 
            Name, 
            Content, 
            data.AuthorId, 
            Tags);

        return NewItem;
    }
}
