using Application.Abstractions.Messaging;

namespace Application.Posts.Queries.GetPostById;
public sealed record GetPostByIdQuery(Guid Id) : IQuery<PostResponse>;
