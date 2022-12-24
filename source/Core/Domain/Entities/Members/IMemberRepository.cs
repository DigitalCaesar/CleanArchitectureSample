using Domain.ValueObjects;

namespace Domain.Entities.Members;
public interface IMemberRepository
{
    Task CreateAsync(MemberEntity member, CancellationToken cancellationToken);
    Task<List<MemberEntity>> GetAll(CancellationToken cancellationToken);
    Task<MemberEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<MemberEntity?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task<MemberEntity?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken);
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);
    Task<bool> IsUsernameUniqueAsync(UserName username, CancellationToken cancellationToken);
}
