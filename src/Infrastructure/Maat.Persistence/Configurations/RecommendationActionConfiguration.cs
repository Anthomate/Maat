using Maat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maat.Persistence.Configurations;

public class RecommendationActionConfiguration : IEntityTypeConfiguration<RecommendationAction>
{
    public void Configure(EntityTypeBuilder<RecommendationAction> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ActionType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Description)
            .HasMaxLength(1000);
    }
}