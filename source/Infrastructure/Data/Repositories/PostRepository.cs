using Domain.Entities.Posts;
using Data.Exceptions;
using Data.Models;
using Data.Mapping;
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

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        //PostData? RawData = await mDataContext.Posts.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        PostData? RawData = await mDataContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        Post ExistingPost = RawData.Map();
        return ExistingPost;
    }

    public async Task CreateAsync(Post post, CancellationToken cancellationToken = default)
    {
        Post? ExistingItem = await GetByIdAsync(post.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Post Id", post.Id.ToString());

        PostData NewPost = post.Map();

        //mDataContext.Entry<MemberData>(NewPost.Author).State = EntityState.Detached;
        await mDataContext.Posts.AddAsync(NewPost);
        await mDataContext.SaveChangesAsync();
    }

    public async Task<List<Post>> GetAll(CancellationToken cancellationToken = default)
    {
        List<PostData> RawData = await mDataContext.Posts.ToListAsync();
        List<Post> MappedData = RawData.Select(x => (Post)x.Map()).ToList();
        return MappedData;
    }
    public async Task<bool> IsNameUniqueAsync(PostName postName, CancellationToken cancellationToken)
    {
        PostData? RawData = await mDataContext.Posts.FirstOrDefaultAsync(x => x.Name == postName.Value);
        return (RawData is null);
    }
}
