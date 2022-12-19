using Domain.Entities.Posts;
using Data.Exceptions;
using Data.Models;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Members;

namespace Data.Repositories;
public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext mDataContext;

    public MemberRepository(ApplicationDbContext dbContext)
    {
        mDataContext = dbContext;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        MemberData? RawData = await mDataContext.Members.FirstOrDefaultAsync(x => x.Id == id);
        if (RawData is null)
            return default;

        Member ExistingPost = RawData.Map();
        return ExistingPost;
    }

    public async Task CreateAsync(Member member, CancellationToken cancellationToken = default)
    {
        Member? ExistingItem = await GetByIdAsync(member.Id, cancellationToken);
        if (ExistingItem is not null)
            throw new DuplicateDataException("Member Id", member.Id.ToString());

        MemberData NewMember = member.Map();

        await mDataContext.Members.AddAsync(NewMember);
        await mDataContext.SaveChangesAsync();
    }
}
