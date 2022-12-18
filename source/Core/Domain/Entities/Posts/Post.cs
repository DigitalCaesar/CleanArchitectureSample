using Domain.Entities.Members;
using Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Posts;

/// <summary>
/// Content to post for public view
/// </summary>
public sealed class Post : Entity
{
    private readonly List<Tag> mTags = new();

    /// <summary>
    /// The title of the post
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The content of the post
    /// </summary>
    public string Content { get; private set; }
    /// <summary>
    /// The member that created or owns the post
    /// </summary>
    public Member Author { get; private set; }
    /// <summary>
    /// A list of categories associated to the post
    /// </summary>
    public ReadOnlyCollection<Tag> Tags => mTags.AsReadOnly();

    /// <summary>
    /// Constructor requires values for all properties
    /// </summary>
    /// <param name="id">the unique identifer to index the Post</param>
    /// <param name="name">the title of the post</param>
    /// <param name="content">the article of the post</param>
    /// <param name="author">the author or owner of the post</param>
    private Post(Guid id, string name, string content, Member author, List<Tag> tags) : base(id)
    {
        Name = name;
        Content = content;
        Author = author;
        mTags = tags;
    }

    /// <summary>
    /// Creates a new post automatically adding an Id
    /// </summary>
    /// <param name="name">the title of the post</param>
    /// <param name="content">the article of the post</param>
    /// <param name="author">the author or owner of the post</param>
    /// <returns>a post</returns>
    public static Post Create(string name, string content, Member author, List<Tag> tags)
    {
        var id = Guid.NewGuid();
        return new Post(id, name, content, author, tags);
    }
    /// <summary>
    /// Creates a new post from existing post data
    /// </summary>
    /// <param name="id">the unique identifer to index the Post</param>
    /// <param name="name">the title of the post</param>
    /// <param name="content">the article of the post</param>
    /// <param name="author">the author or owner of the post</param>
    /// <returns>a post</returns>
    public static Post Create(Guid id, string name, string content, Member author, List<Tag> tags)
    {
        return new Post(id, name, content, author, tags);
    }

    /// <summary>
    /// A category to associate to the Post
    /// </summary>
    /// <param name="tag">a category to add</param>
    public void AddTag(Tag tag)
    {
        var ExistingTag = mTags.Find(x => x.Name == tag.Name);
        if(ExistingTag is null)
            mTags.Add(tag);
    }
    /// <summary>
    /// An existing category to disassociate to the Post
    /// </summary>
    /// <param name="tag">a category to remove</param>
    public void RemoveTag(Tag tag)
    {
        var ExistingTag = mTags.Find(x => x.Name == tag.Name);
        if(ExistingTag is not null)
            mTags.Remove(tag);
    }
}
