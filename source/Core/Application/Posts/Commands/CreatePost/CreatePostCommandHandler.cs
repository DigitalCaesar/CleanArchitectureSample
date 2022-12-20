using Application.Abstractions.Messaging;
using Domain;
using Domain.Entities.Posts;
using Domain.Exceptions;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Posts.Commands.CreatePost;
internal sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid> 
{
    private readonly IPostRepository mPostRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    {
        mPostRepository = postRepository;
        mUnitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        var postName = PostName.Create(request.Name);
        if (!postName.Successful || postName.Value is null)
            return Result<Guid>.Failure(new Error("PostName", $"The requested name '{request.Name}' is invalid."));
        var postContent = PostContent.Create(request.Content);
        if (!postContent.Successful || postContent.Value is null)
            return Result<Guid>.Failure(new Error("PostContent", $"The requested post content '{request.Content}' is invalid."));


        var NewPost = Post.Create(
            postName.Value, 
            postContent.Value, 
            request.Author, 
            request.Tags);

        await mPostRepository.CreateAsync(NewPost, cancellationToken);

        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(NewPost.Id);
    }
}
