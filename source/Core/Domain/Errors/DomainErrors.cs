using Domain.Shared;
using System.Security.Cryptography.X509Certificates;

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
            "Member.DuplicateEmail",
            $"The email address '{email}' is already in use and cannot be duplicated.");
        public static Error DuplicateUsername(string username) => new(
            "Member.DuplicateUsername",
            $"The username '{username}' is already in use and cannot be duplicated.");
        public static Error InvalidCredentials => new(
            "Member.InvalidCredentials",
            $"The credentials provided could not be authenticated.");
    }
    public static class Email
    {
        public static Error Empty => new(
            $"Email.Empty",
            $"The email address must have a value.");
        public static Error TooLong(int maxLength) => new(
            $"Email.TooLong",
            $"The email address must be less that {maxLength} characters in length.");
        public static Error InvalidFormat => new(
            $"Email.InvalidFormat", 
            $"The email address must be an email address with the format name@domain.ext");
    }
}
