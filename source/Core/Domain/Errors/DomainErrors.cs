using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors;
public static class DomainErrors
{
    public static class Post
    {
        public static Error PostNotFound(Guid postId) => new(
            "PostNotFound",
            $"The post with the identifier {postId} was not found.");
    }
}
