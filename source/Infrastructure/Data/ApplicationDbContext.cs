using Data.Models;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Roles;
using Domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) 
        : base(options) 
    {
        //HACK:  Temp fix to make sure in memory DB is seeded
        Database.EnsureCreated();
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public DbSet<Member> Members { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<DomainEventData> DomainEvents { get; set; }
    public DbSet<EventConsumer> DomainEventConsumer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Data.AssemblyReference.Assembly);
    }
}
