
using Domain.Entities.Tags;

namespace Domain.Entities.Posts;

public class Post
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    //[ForeignKey(nameof(AuthorId))]
    //public virtual MemberData Author { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}