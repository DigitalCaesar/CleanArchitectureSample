using Application.Abstractions.Messaging;
using Domain;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Tags;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Posts.Commands.CreatePost;
internal sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid> 
{
    private readonly IPostRepository mPostRepository;
    private readonly ITagRepository mTagRepository;
    private readonly IMemberRepository mMemberRepository;
    private readonly IEventRepository mEventRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository, ITagRepository tagRepository, IMemberRepository memberRepository)
    {
        mPostRepository = postRepository;
        mUnitOfWork = unitOfWork;
        mEventRepository = eventRepository;
        mTagRepository = tagRepository;
        mMemberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        // Check values
        var postName = PostName.Create(request.Name);
        if (!postName.Successful || postName.Value is null)
            return Result.Failure<Guid>(new Error("PostName", $"The requested name '{request.Name}' is invalid."));
        var postContent = PostContent.Create(request.Content);
        if (!postContent.Successful || postContent.Value is null)
            return Result.Failure<Guid>(new Error("PostContent", $"The requested post content '{request.Content}' is invalid."));

        // Check uniqueness
        if (!await mPostRepository.IsNameUniqueAsync(postName.Value, cancellationToken))
            return Result.Failure<Guid>(new Error("Name", "The Name must be unique"));

        // Set Tags
        List<Tag> Tags = new();
        foreach(var tag in request.Tags)
        {
            Tag? FoundItem = await mTagRepository.GetByName(tag, cancellationToken);
            if (FoundItem is not null)
                Tags.Add(FoundItem);
            else
            {
                Result<Name> TagName = Name.Create(tag);
                Result<Description> TagDescription = Description.Create(tag);                

                if(TagName.Successful && TagDescription.Successful)
                    Tags.Add(Tag.Create(TagName.Value!, TagDescription.Value!));
            }
        }

        // Get Author
        Guid AuthorId = Guid.Parse(request.AuthorId);
        Member? Author = await mMemberRepository.GetByIdAsync(AuthorId, cancellationToken);
        if (Author is null)
            return Result.Failure<Guid>(new Error("PostAuthor", "A Post must have a valid, existing author."));

        // Create the new item
        var NewItem = Post.Create(
            postName.Value, 
            postContent.Value, 
            AuthorId, 
            Tags);

        // Save the item
        await mPostRepository.CreateAsync(NewItem, cancellationToken);
        await mEventRepository.AddEventAsync(NewItem, cancellationToken);
        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(NewItem.Id);
    }
}
