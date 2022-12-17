using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;
public sealed class PostNotFoundException : NotFoundException
{
    public PostNotFoundException(Guid postId)
        : base($"The post with the identifier {postId} was not found.")
    { }
}
