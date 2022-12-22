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
            "DuplicateEmail",
            $"The email address '{email}' is already in use and cannot be duplicated.");

        public static Error DuplicateUsername(string username) => new(
            "DuplicateUsername",
            $"The username '{username}' is already in use and cannot be duplicated.");
    }
    public static class Email
    {
        public static Error Empty(string shortName, string longName) => new(
            $"{shortName}.Empty",
            $"The {longName} must have a value.");
        public static Error TooLong(string shortName, string longName, int maxLength) => new(
            $"{shortName}.TooLong",
            $"The {longName} must be less that {maxLength} characters in length.");
        public static Error InvalidFormat(string shortName, string longName) => new(
            $"{shortName}.InvalidFormat", 
            $"The {longName} must be an email address with the format name@domain.ext");
    }
}
