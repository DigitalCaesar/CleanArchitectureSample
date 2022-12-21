

using Domain.ValueObjects;

namespace Domain.Entities.Posts;
public interface IPostRepository
{
    Task CreateAsync(Post post, CancellationToken cancellationToken);
    Task<List<Post>> GetAll(CancellationToken cancellationToken);
    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(PostName name, CancellationToken cancellationToken);
}
