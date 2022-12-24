using Domain.Entities.Posts;

namespace Domain.Entities.Tags;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}