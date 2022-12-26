using Data;
using Domain.Entities.Members;
using Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication;
public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext mDbContext;

    public PermissionService(ApplicationDbContext dbContext)
    {
        mDbContext = dbContext;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(Guid memberId)
    {
        ICollection<Role>[] roles = await mDbContext.Set<Member>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == memberId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}
