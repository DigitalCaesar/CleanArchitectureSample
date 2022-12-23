using Domain.Entities.Posts;
using Data.Exceptions;
using Data.Models;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Members;
using Domain.ValueObjects;

namespace Data.Repositories;
public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext mDataContext;

    public MemberRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }


    public async Task CreateAsync(Member member, CancellationToken cancellationToken = default)
    {
        Member? ExistingItem = await GetByIdAsync(member.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Member Id", member.Id.ToString());

        MemberData MappedItem = member.Map();

        await mDataContext.Members.AddAsync(MappedItem);
        await mDataContext.SaveChangesAsync();
    }
    public async Task<List<Member>> GetAll(CancellationToken cancellationToken = default)
    {
        List<MemberData> RawData = await mDataContext.Members.Include(x => x.Roles).ToListAsync();
        List<Member> MappedData = RawData.Select(x => (Member)x.Map()).ToList();
        return MappedData;
    }
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        MemberData? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        Member MappedItem = RawData.Map();
        return MappedItem;
    }
    public async Task<Member?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        MemberData? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Email == email.Value);
        if (RawData is null)
            return default;

        Member MappedItem = RawData.Map();
        return MappedItem;
    }
    public async Task<Member?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken = default)
    {
        MemberData? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Username == username.Value);
        if (RawData is null)
            return default;

        Member MappedItem = RawData.Map();
        return MappedItem;
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        MemberData? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Email == email.Value);
        return (RawData is null);
    }
    public async Task<bool> IsUsernameUniqueAsync(UserName username, CancellationToken cancellationToken)
    {
        MemberData? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Username == username.Value);
        return (RawData is null);
    }

}
