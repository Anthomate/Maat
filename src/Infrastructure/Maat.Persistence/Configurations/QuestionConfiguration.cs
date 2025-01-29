using Maat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maat.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(q => q.Description)
            .HasMaxLength(2000);

        builder.Property(q => q.Type)
            .IsRequired();

        builder.Property(q => q.Category)
            .IsRequired();

        builder.Property(q => q.Order)
            .IsRequired();

        builder.Property(q => q.Weight)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.HasMany(q => q.Options)
            .WithOne()
            .HasForeignKey("QuestionId");

        builder.HasIndex(q => new { q.Category, q.Order });
    }
}