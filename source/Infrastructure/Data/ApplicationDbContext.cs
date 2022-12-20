using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) 
        : base(options) { }

    public DbSet<PostData> Posts { get; set; }
    public DbSet<TagData> Tags { get; set; }

    public DbSet<MemberData> Members { get; set; }
    public DbSet<RoleData> Roles { get; set; }

    public DbSet<DomainEventData> DomainEvents { get; set; }
}
