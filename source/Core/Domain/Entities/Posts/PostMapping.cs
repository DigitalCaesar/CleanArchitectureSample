using Domain.Entities.Tags;
using Domain.ValueObjects;

namespace Domain.Entities.Posts;
public static class PostMapping
{
    public static Post Map(this PostEntity entity)
    {
        List<Tag> Tags = entity.Tags.Select(x => (Tag)x.Map()).ToList();

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

        if (
            Name is null ||
            Content is null)
            throw new Domain.Exceptions.EntityMappingException("PostData");

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
