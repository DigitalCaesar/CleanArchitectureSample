using Domain.Entities.Posts;
using Data.Exceptions;
using Data.Models;
using Data.Mapping;
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

    public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TagData? RawData = await mDataContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        Tag ExistingPost = RawData.Map();
        return ExistingPost;
    }

    public async Task CreateAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        Tag? ExistingItem = await GetByIdAsync(tag.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Tag Id", tag.Id.ToString());

        TagData NewItem = tag.Map();

        await mDataContext.Tags.AddAsync(NewItem);
        await mDataContext.SaveChangesAsync();
    }

    public async Task<List<Tag>> GetAll(CancellationToken cancellationToken = default)
    {
        List<TagData> RawData = await mDataContext.Tags.ToListAsync();
        List<Tag> MappedData = RawData.Select(x => (Tag)x.Map()).ToList();
        return MappedData;
    }
    public async Task<bool> IsNameUniqueAsync(Name name, CancellationToken cancellationToken)
    {
        TagData? RawData = await mDataContext.Tags.FirstOrDefaultAsync(x => x.Name == name.Value);
        return (RawData is null);
    }

    public async Task<Tag?> GetByName(string name, CancellationToken cancellationToken)
    {
        TagData? RawData = await mDataContext.Tags.FirstOrDefaultAsync(x => x.Name == name);
        Tag? MappedData = (RawData is not null) ? RawData.Map() : default!;
        return MappedData;
    }
}
