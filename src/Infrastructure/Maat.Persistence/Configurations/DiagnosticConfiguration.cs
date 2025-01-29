using Maat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maat.Persistence.Configurations;

public class DiagnosticConfiguration : IEntityTypeConfiguration<Diagnostic>
{
    public void Configure(EntityTypeBuilder<Diagnostic> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Status)
            .IsRequired();

        builder.OwnsOne(d => d.EnvironmentalScore);
        builder.OwnsOne(d => d.SocialScore);
        builder.OwnsOne(d => d.EconomicScore);
        builder.OwnsOne(d => d.GovernanceScore);

        builder.HasOne(d => d.Company)
            .WithMany(c => c.Diagnostics)
            .HasForeignKey(d => d.CompanyId)
            .IsRequired();

        builder.HasMany(d => d.Recommendations)
            .WithOne()
            .HasForeignKey("DiagnosticId");
    }
}