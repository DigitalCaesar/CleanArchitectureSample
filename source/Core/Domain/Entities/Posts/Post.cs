using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Posts;
public sealed class Post : BaseEntity
{
    public string Name { get; private set; }

    public Post(Guid id, string name) : base(id)
    {
        Name = name;
    }
    private Post() { }
}
