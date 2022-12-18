using Domain;
using Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Commands.CreatePost;
internal sealed class CreatePostCommandHandler // : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository mPostRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    {
        mPostRepository = postRepository;
        mUnitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var NewPost = Post.Create(request.Name, request.Content, request.Author, request.Tags);

        mPostRepository.Insert(NewPost);

        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return NewPost.Id;
    }
}
