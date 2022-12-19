using Domain;
using Domain.Entities.Posts;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Commands.CreatePost;
internal sealed class CreatePostCommandHandler //: IRequestHandler<CreatePostCommand, Guid> // : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository mPostRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    {
        mPostRepository = postRepository;
        mUnitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        var postName = PostName.Create(request.Name);
        if (!postName.Successful || postName.Value is null)
            throw new ValidationException($"The requested name '{request.Name}' is invalid."); //TODO: Add Error info

        var postContent = PostContent.Create(request.Content);
        if (!postContent.Successful || postContent.Value is null)
            throw new ValidationException($"The requested post content '{request.Content}' is invalid."); //TODO: Add Error info


        var NewPost = Post.Create(
            postName.Value, 
            postContent.Value, 
            request.Author, 
            request.Tags);

        await mPostRepository.CreateAsync(NewPost, cancellationToken);

        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return NewPost.Id;
    }
}
