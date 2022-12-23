using Data.Mapping;
using Data.Models;
using MediatR.Wrappers;
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

    public DbSet<PostData> Posts { get; set; }
    public DbSet<TagData> Tags { get; set; }

    public DbSet<MemberData> Members { get; set; }
    public DbSet<RoleData> Roles { get; set; }

    public DbSet<DomainEventData> DomainEvents { get; set; }
    public DbSet<EventConsumer> DomainEventConsumer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TagData>(tagBuilder =>
        {
            tagBuilder
                .HasKey(t => t.Id);
            tagBuilder
                .HasData(new TagData
                {
                    Id = Guid.Parse("00000002-0000-0000-0000-000000000001"),
                    Name = "Test",
                    Description = "Test Category"
                });
        });

        modelBuilder.Entity<PostData>(postBuilder =>
        {
            postBuilder.HasKey(p => p.Id);

            //postBuilder
            //    .HasMany(p => p.Tags)
            //    .WithMany();
            postBuilder
                .HasData(new PostData
                {
                    Id = Guid.Parse("00000001-0000-0000-0000-000000000001"),
                    Name = "TestPost",
                    Content = "This is the content of the first test post.",
                    AuthorId = Guid.Parse("00000003-0000-0000-0000-000000000002")
                });
            postBuilder
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity(j => j.HasData(new 
                { 
                    PostsId = Guid.Parse("00000001-0000-0000-0000-000000000001"), 
                    TagsId = Guid.Parse("00000002-0000-0000-0000-000000000001")
                }));
        });

        modelBuilder.Entity<RoleData>(roleBuilder =>
        {
            roleBuilder
                .HasKey(t => t.Id);
            roleBuilder
                .HasData(new RoleData
                {
                    Id = Guid.Parse("00000004-0000-0000-0000-000000000001"),
                    Name = "Reader",
                    Description = "Read only general user role"
                },
                new RoleData
                {
                    Id = Guid.Parse("00000004-0000-0000-0000-000000000002"),
                    Name = "Author",
                    Description = "Read/Write priviledged user role"
                },
                new RoleData
                {
                    Id = Guid.Parse("00000004-0000-0000-0000-000000000003"),
                    Name = "Admin",
                    Description = "Administrator role"
                });
        });
        modelBuilder.Entity<MemberData>(memberBuilder =>
        {
            memberBuilder
                .HasKey(m => m.Id);
            //memberBuilder
            //    .HasMany(m => m.Roles)
            //    .WithMany();
            memberBuilder
                .HasData(new MemberData
                {
                    Id = Guid.Parse("00000003-0000-0000-0000-000000000001"),
                    Username = "User",
                    Email = "user@test.com",
                    FirstName = "First",
                    LastName = "User"
                },
                new MemberData
                {
                    Id = Guid.Parse("00000003-0000-0000-0000-000000000002"),
                    Username = "Author",
                    Email = "Author@test.com",
                    FirstName = "Second",
                    LastName = "User"
                },
                new MemberData
                {
                    Id = Guid.Parse("00000003-0000-0000-0000-000000000003"),
                    Username = "Admin",
                    Email = "Admin@test.com",
                    FirstName = "Third",
                    LastName = "User"
                });
            memberBuilder
                .HasMany(p => p.Roles)
                .WithMany(t => t.Members)
                .UsingEntity(j => j.HasData(
                    new
                    {
                        MembersId = Guid.Parse("00000003-0000-0000-0000-000000000001"),
                        RolesId = Guid.Parse("00000004-0000-0000-0000-000000000001")
                    },
                    new
                    {
                        MembersId = Guid.Parse("00000003-0000-0000-0000-000000000002"),
                        RolesId = Guid.Parse("00000004-0000-0000-0000-000000000002")
                    },
                    new
                    {
                        MembersId = Guid.Parse("00000003-0000-0000-0000-000000000003"),
                        RolesId = Guid.Parse("00000004-0000-0000-0000-000000000003")
                    }));
        });
    }
}
