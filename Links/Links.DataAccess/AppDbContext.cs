using Links.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Links.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Link> Links { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Link>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Url).IsRequired();
        });
    }
}