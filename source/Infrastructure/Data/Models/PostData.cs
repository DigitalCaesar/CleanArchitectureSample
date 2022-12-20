using Domain.Entities.Members;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;

[Table("Posts")]
public class PostData
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public MemberData? Author { get; set; }
    public virtual ICollection<TagData> Tags { get; set; } = new List<TagData>();
}
