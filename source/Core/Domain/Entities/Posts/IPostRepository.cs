

using Domain.ValueObjects;

namespace Domain.Entities.Posts;
public interface IPostRepository
{
    Task CreateAsync(PostEntity post, CancellationToken cancellationToken);
    Task<List<PostEntity>> GetAll(CancellationToken cancellationToken);
    Task<PostEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(PostName name, CancellationToken cancellationToken);
}
