using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAccessControl.Core.Entities;

namespace UserAccessControl.Core.Database;

public class AppDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ResourceEntity> Resources { get; set; }
    public DbSet<AccessGrantEntity> AccessGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ResourceEntity>(entity => {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(r => r.Description)
                .HasMaxLength(500);
            entity.HasMany(r => r.AccessGrants)
                .WithOne(ag => ag.Resource)
                .HasForeignKey(ag => ag.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<AccessGrantEntity>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne(a => a.User)
                .WithMany(u => u.AccessGrants)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(a => a.Resource)
                .WithMany(r => r.AccessGrants)
                .HasForeignKey(a => a.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(a => a.AccessLevel)
                .IsRequired();
        });

        builder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);
            entity.HasMany(u => u.AccessGrants)
                .WithOne(ag => ag.User)
                .HasForeignKey(ag => ag.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
