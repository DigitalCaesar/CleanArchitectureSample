using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

[Table("Posts")]
public class PostData
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    //[ForeignKey(nameof(AuthorId))]
    //public virtual MemberData Author { get; set; }
    public ICollection<TagData> Tags { get; set; } = new List<TagData>();
}