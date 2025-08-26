using Insurance.Infrastructure.Persistence.Entities;
using Insurance.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Persistence.Configurations;

public class InsurancePolicyEntityConfig : IEntityTypeConfiguration<InsurancePolicyEntity>
{
    public void Configure(EntityTypeBuilder<InsurancePolicyEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("InsurancePolicies", tb =>
        {
            tb.HasCheckConstraint(
                "CK_InsurancePolicies_CarRequiresReg",
                "\"Type\" <> 3 OR \"VehicleRegNumber\" IS NOT NULL");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.PersonalNumber)
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(x => x.PersonalNumber);

        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.MonthlyCostAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.MonthlyCostCurrency)
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.VehicleRegNumber)
            .HasMaxLength(32);

        builder.HasIndex(x => new { x.PersonalNumber, x.VehicleRegNumber })
            .HasFilter("\"Type\" = 3 AND \"VehicleRegNumber\" IS NOT NULL")
            .IsUnique();

        builder.HasData(InsurancePolicySeed.Initial);
    }
}
