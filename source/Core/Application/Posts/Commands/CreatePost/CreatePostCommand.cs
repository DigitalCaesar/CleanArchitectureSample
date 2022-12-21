using Application.Abstractions.Messaging;
using Domain.Entities.Members;

namespace Application.Posts.Commands.CreatePost;
public sealed record CreatePostCommand(
    string Name, 
    string Content, 
    string AuthorId, 
    List<string> Tags) : ICommand<Guid>; 