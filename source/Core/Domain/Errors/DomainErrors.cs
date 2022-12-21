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
    public static class Member
    {
        public static Error DuplicateEmail(string email) => new(
            "DuplicateEmail",
            $"The email address '{email}' is already in use and cannot be duplicated.");

        public static Error DuplicateUsername(string username) => new(
            "DuplicateUsername",
            $"The username '{username}' is already in use and cannot be duplicated.");
    }
}
