using Maat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maat.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Siret)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(c => c.Siret)
            .IsUnique();

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(200);
            address.Property(a => a.City).HasMaxLength(100);
            address.Property(a => a.PostalCode).HasMaxLength(10);
            address.Property(a => a.Country).HasMaxLength(100);
        });

        builder.HasMany(c => c.Users)
            .WithOne(u => u.Company)
            .HasForeignKey("CompanyId");

        builder.HasMany(c => c.Diagnostics)
            .WithOne(d => d.Company)
            .HasForeignKey(d => d.CompanyId);
    }
}