using Maat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maat.Persistence.Configurations;

public class QuestionResponseConfiguration : IEntityTypeConfiguration<QuestionResponse>
{
    public void Configure(EntityTypeBuilder<QuestionResponse> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Response)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Score)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.HasIndex(r => new { r.DiagnosticId, r.QuestionId }).IsUnique();
    }
}