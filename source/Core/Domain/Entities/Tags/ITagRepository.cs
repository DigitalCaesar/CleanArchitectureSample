

namespace Domain.Entities.Tags;
public interface ITagRepository
{
    Task CreateAsync(Tag item, CancellationToken cancellationToken);
    Task<List<Tag>> GetAll(CancellationToken cancellationToken);
    Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Tag?> GetByName(string name, CancellationToken cancellationToken);
}
