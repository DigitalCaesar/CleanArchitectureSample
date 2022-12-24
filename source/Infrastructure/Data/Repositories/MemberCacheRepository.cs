using Domain.Entities.Members;
using Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;
public class MemberCacheRepository : IMemberRepository
{
    private const int cExpiration = 2;

    private readonly MemberRepository mMemberRepository;
    private readonly IMemoryCache mMemoryCache;

    public MemberCacheRepository(IMemoryCache memoryCache, MemberRepository memberRepository)
    {
        mMemoryCache = memoryCache;
        mMemberRepository = memberRepository;
    }

    public Task CreateAsync(MemberEntity member, CancellationToken cancellationToken)
    {
        return mMemberRepository.CreateAsync(member, cancellationToken);
    }

    public Task<List<MemberEntity>> GetAll(CancellationToken cancellationToken)
    {
        return mMemberRepository.GetAll(cancellationToken);
    }


    public Task<MemberEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        string key = $"member-{id}";

        return mMemoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(cExpiration));

                return mMemberRepository.GetByIdAsync(id, cancellationToken);
            });
    }
    public Task<MemberEntity?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return mMemberRepository.GetByEmailAsync(email, cancellationToken);
    }

    public Task<MemberEntity?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken)
    {
        return mMemberRepository.GetByUsernameAsync(username, cancellationToken);
    }

    public Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken)
    {
        return mMemberRepository.IsEmailUniqueAsync(email, cancellationToken);
    }

    public Task<bool> IsUsernameUniqueAsync(UserName username, CancellationToken cancellationToken)
    {
        return mMemberRepository.IsUsernameUniqueAsync(username, cancellationToken);
    }
}
