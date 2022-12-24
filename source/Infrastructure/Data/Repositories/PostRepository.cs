using Domain.Entities.Posts;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;

namespace Data.Repositories;
public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext mDataContext;

    public PostRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }

    public async Task<PostEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        //PostData? RawData = await mDataContext.Posts.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        Post? RawData = await mDataContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        PostEntity ExistingPost = RawData.Map();
        return ExistingPost;
    }

    public async Task CreateAsync(PostEntity post, CancellationToken cancellationToken = default)
    {
        PostEntity? ExistingItem = await GetByIdAsync(post.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Post Id", post.Id.ToString());

        Post NewPost = post.Map();

        await mDataContext.Posts.AddAsync(NewPost);
    }

    public async Task<List<PostEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        List<Post> RawData = await mDataContext.Posts.Include(x => x.Tags).ToListAsync();
        List<PostEntity> MappedData = RawData.Select(x => (PostEntity)x.Map()).ToList();
        return MappedData;
    }
    public async Task<bool> IsNameUniqueAsync(PostName postName, CancellationToken cancellationToken)
    {
        Post? RawData = await mDataContext.Posts.FirstOrDefaultAsync(x => x.Name == postName.Value);
        return (RawData is null);
    }
}
