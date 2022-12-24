using Domain.Entities.Posts;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using Domain.Entities.Tags;

namespace Data.Repositories;
public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext mDataContext;

    public TagRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }

    public async Task<TagEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Tag? RawData = await mDataContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        TagEntity ExistingPost = RawData.Map();
        return ExistingPost;
    }

    public async Task CreateAsync(TagEntity tag, CancellationToken cancellationToken = default)
    {
        TagEntity? ExistingItem = await GetByIdAsync(tag.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Tag Id", tag.Id.ToString());

        Tag NewItem = tag.Map();

        await mDataContext.Tags.AddAsync(NewItem);
    }

    public async Task<List<TagEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        List<Tag> RawData = await mDataContext.Tags.ToListAsync();
        List<TagEntity> MappedData = RawData.Select(x => (TagEntity)x.Map()).ToList();
        return MappedData;
    }
    public async Task<bool> IsNameUniqueAsync(Name name, CancellationToken cancellationToken)
    {
        Tag? RawData = await mDataContext.Tags.FirstOrDefaultAsync(x => x.Name == name.Value);
        return (RawData is null);
    }

    public async Task<TagEntity?> GetByName(string name, CancellationToken cancellationToken)
    {
        Tag? RawData = await mDataContext.Tags.FirstOrDefaultAsync(x => x.Name == name);
        TagEntity? MappedData = (RawData is not null) ? RawData.Map() : default!;
        return MappedData;
    }
}
