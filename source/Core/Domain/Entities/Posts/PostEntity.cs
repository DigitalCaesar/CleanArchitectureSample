using Domain.Entities.Members;
using Domain.Entities.Tags;
using Domain.Events;
using Domain.Shared;
using Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Domain.Entities.Posts;

/// <summary>
/// Content to post for public view
/// </summary>
public sealed class PostEntity : AggregateRoot
{
    private readonly List<TagEntity> mTags = new();

    /// <summary>
    /// The title of the post
    /// </summary>
    public PostName Name { get; private set; }
    /// <summary>
    /// The content of the post
    /// </summary>
    public PostContent Content { get; private set; }
    /// <summary>
    /// The member that created or owns the post
    /// </summary>
    public Guid AuthorId { get; private set; }
    /// <summary>
    /// A list of categories associated to the post
    /// </summary>
    public ReadOnlyCollection<TagEntity> Tags => mTags.AsReadOnly();

    /// <summary>
    /// Constructor requires values for all properties
    /// </summary>
    /// <param name="id">the unique identifer to index the Post</param>
    /// <param name="name">the title of the post</param>
    /// <param name="content">the article of the post</param>
    /// <param name="author">the author or owner of the post</param>
    private PostEntity(Guid id, PostName name, PostContent content, Guid authorId, List<TagEntity> tags) : base(id)
    {
        Name = name;
        Content = content;
        AuthorId = authorId;
        mTags = tags;

        RaiseDomainEvent(new PostCreatedEvent(id, authorId));
    }

    /// <summary>
    /// Creates a new post automatically adding an Id
    /// </summary>
    /// <param name="name">the title of the post</param>
    /// <param name="content">the article of the post</param>
    /// <param name="author">the author or owner of the post</param>
    /// <returns>a post</returns>
    public static PostEntity Create(PostName name, PostContent content, Guid authorId, List<TagEntity> tags)
    {
        var id = Guid.NewGuid();
        return new PostEntity(id, name, content, authorId, tags);
    }
    /// <summary>
    /// Creates a new post from existing post data
    /// </summary>
    /// <param name="id">the unique identifer to index the Post</param>
    /// <param name="name">the title of the post</param>
    /// <param name="content">the article of the post</param>
    /// <param name="author">the author or owner of the post</param>
    /// <returns>a post</returns>
    public static PostEntity Create(Guid id, PostName name, PostContent content, Guid authorId, List<TagEntity> tags)
    {
        return new PostEntity(id, name, content, authorId, tags);
    }

    /// <summary>
    /// A category to associate to the Post
    /// </summary>
    /// <param name="tag">a category to add</param>
    public void AddTag(TagEntity tag)
    {
        var ExistingTag = mTags.Find(x => x.Name == tag.Name);
        if(ExistingTag is null)
            mTags.Add(tag);
    }
    /// <summary>
    /// An existing category to disassociate to the Post
    /// </summary>
    /// <param name="tag">a category to remove</param>
    public void RemoveTag(TagEntity tag)
    {
        var ExistingTag = mTags.Find(x => x.Name == tag.Name);
        if(ExistingTag is not null)
            mTags.Remove(tag);
    }
}
