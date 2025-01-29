using Maat.Domain.Entities;
using Maat.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Maat.Persistence.Context;

public class MaatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Diagnostic> Diagnostics { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionResponse> QuestionResponses { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<RecommendationAction> RecommendationActions { get; set; }

    public MaatDbContext(DbContextOptions<MaatDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaatDbContext).Assembly);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}