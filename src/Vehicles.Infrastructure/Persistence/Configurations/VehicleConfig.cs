using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicles.Infrastructure.Persistence.Entities;
using Vehicles.Infrastructure.Persistence.Seed;

namespace Vehicles.Infrastructure.Persistence.Configurations;

public class VehicleConfig : IEntityTypeConfiguration<VehicleEntity>
{
    public void Configure(EntityTypeBuilder<VehicleEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Vehicles");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).ValueGeneratedNever();


        builder.Property(v => v.RegistrationNumber)
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(v => v.RegistrationNumber).IsUnique();

        builder.Property(v => v.Make).HasMaxLength(64).IsRequired();
        builder.Property(v => v.Model).HasMaxLength(64).IsRequired();
        builder.Property(v => v.Year).IsRequired();

        builder.Property(v => v.Vin).HasMaxLength(32).IsRequired();
        builder.HasIndex(v => v.Vin).IsUnique();

        builder.HasData(VehicleSeed.Initial);
    }
}
