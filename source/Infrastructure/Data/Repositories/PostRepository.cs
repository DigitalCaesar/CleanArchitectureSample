using Domain.Entities.Posts;
using Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;

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

        await mDataContext.Posts.AddAsync(NewPost);
        await mDataContext.SaveChangesAsync();
    }
}
