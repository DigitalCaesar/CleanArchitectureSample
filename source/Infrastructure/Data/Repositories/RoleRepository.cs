using Domain.Entities.Posts;
using Data.Exceptions;
using Data.Models;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using Domain.Entities.Tags;
using Domain.Entities.Roles;

namespace Data.Repositories;
public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext mDataContext;

    public RoleRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        RoleData? RawData = await mDataContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        Role ExistingPost = RawData.Map();
        return ExistingPost;
    }

    public async Task CreateAsync(Role role, CancellationToken cancellationToken = default)
    {
        Role? ExistingItem = await GetByIdAsync(role.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Role Id", role.Id.ToString());

        RoleData NewItem = role.Map();

        await mDataContext.Roles.AddAsync(NewItem);
        await mDataContext.SaveChangesAsync();
    }

    public async Task<List<Role>> GetAll(CancellationToken cancellationToken = default)
    {
        List<RoleData> RawData = await mDataContext.Roles.ToListAsync();
        List<Role> MappedData = RawData.Select(x => (Role)x.Map()).ToList();
        return MappedData;
    }
    public async Task<bool> IsNameUniqueAsync(Name name, CancellationToken cancellationToken)
    {
        RoleData? RawData = await mDataContext.Roles.FirstOrDefaultAsync(x => x.Name == name.Value);
        return (RawData is null);
    }

    public async Task<Role?> GetByName(string name, CancellationToken cancellationToken)
    {
        RoleData? RawData = await mDataContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
        Role? MappedData = (RawData is not null) ? RawData.Map() : default!;
        return MappedData;
    }
}
