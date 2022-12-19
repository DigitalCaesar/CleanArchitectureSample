using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) 
        : base(options) { }

    public DbSet<PostData> Posts { get; set; }
    public DbSet<TagData> Tags { get; set; }

    public DbSet<MemberData> Members { get; set; }
    public DbSet<RoleData> Roles { get; set; }
}
