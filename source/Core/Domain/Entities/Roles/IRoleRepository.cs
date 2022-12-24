

namespace Domain.Entities.Roles;
public interface IRoleRepository
{
    Task CreateAsync(Role item, CancellationToken cancellationToken);
    Task<List<Role>> GetAll(CancellationToken cancellationToken);
    Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Role?> GetByName(string name, CancellationToken cancellationToken);
}
