

namespace Domain.Entities.Tags;
public interface ITagRepository
{
    Task CreateAsync(TagEntity item, CancellationToken cancellationToken);
    Task<List<TagEntity>> GetAll(CancellationToken cancellationToken);
    Task<TagEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TagEntity?> GetByName(string name, CancellationToken cancellationToken);
}
