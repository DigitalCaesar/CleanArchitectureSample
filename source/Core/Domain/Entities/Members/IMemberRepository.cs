using Domain.ValueObjects;

namespace Domain.Entities.Members;
public interface IMemberRepository
{
    Task CreateAsync(Member member, CancellationToken cancellationToken);
    Task<List<Member>> GetAll(CancellationToken cancellationToken);
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Member?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task<Member?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken);
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);
    Task<bool> IsUsernameUniqueAsync(UserName username, CancellationToken cancellationToken);
}
