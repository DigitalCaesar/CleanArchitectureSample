using Domain.Entities.Posts;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Members;
using Domain.ValueObjects;
using Domain.Entities.Roles;
using Domain.Entities.Permissions;

namespace Data.Repositories;
public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext mDataContext;

    public MemberRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }


    public async Task CreateAsync(MemberEntity member, CancellationToken cancellationToken = default)
    {
        MemberEntity? ExistingItem = await GetByIdAsync(member.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Member Id", member.Id.ToString());

        Member MappedItem = member.Map();

        await mDataContext.Members.AddAsync(MappedItem);
    }
    public async Task<List<MemberEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        List<Member> RawData = await mDataContext.Members.Include(x => x.Roles).ThenInclude(x => x.Permissions).ToListAsync(cancellationToken);
        List<MemberEntity> MappedData = RawData.Select(x => (MemberEntity)x.Map()).ToList();
        return MappedData;
    }
    public async Task<MemberEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Member? RawData = await mDataContext.Members.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (RawData is null)
            return default;

        MemberEntity MappedItem = RawData.Map();
        return MappedItem;
    }
    public async Task<MemberEntity?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        Member? RawData = await mDataContext.Members.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == email.Value, cancellationToken);
        if (RawData is null)
            return default;

        MemberEntity MappedItem = RawData.Map();
        return MappedItem;
    }
    public async Task<MemberEntity?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken = default)
    {
        Member? RawData = await mDataContext.Members.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Username == username.Value, cancellationToken);
        if (RawData is null)
            return default;

        MemberEntity MappedItem = RawData.Map();
        return MappedItem;
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        Member? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Email == email.Value);
        return (RawData is null);
    }
    public async Task<bool> IsUsernameUniqueAsync(UserName username, CancellationToken cancellationToken)
    {
        Member? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Username == username.Value);
        return (RawData is null);
    }

}
