using Insurance.Infrastructure.Persistence.Entities;

namespace Insurance.Infrastructure.Persistence.Seed;

internal static class InsurancePolicySeed
{
    public static readonly InsurancePolicyEntity[] Initial =
    [
        // Person 19650101-1234
        new()
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            PersonalNumber = "19650101-1234",
            Type = 1, // Pet
            MonthlyCostAmount = 10m,
            MonthlyCostCurrency = "USD",
            VehicleRegNumber = null
        },
        new()
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            PersonalNumber = "19650101-1234",
            Type = 2, // PersonalHealth
            MonthlyCostAmount = 20m,
            MonthlyCostCurrency = "USD",
            VehicleRegNumber = null
        },
        new()
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            PersonalNumber = "19650101-1234",
            Type = 3, // Car
            MonthlyCostAmount = 30m,
            MonthlyCostCurrency = "USD",
            VehicleRegNumber = "ABC123"
        },
        new()
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            PersonalNumber = "19650101-1234",
            Type = 3, // Car
            MonthlyCostAmount = 30m,
            MonthlyCostCurrency = "USD",
            VehicleRegNumber = "XYZ999"
        },

        // Person 19700101-1111
        new()
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            PersonalNumber = "19700101-1111",
            Type = 1, // Pet
            MonthlyCostAmount = 10m,
            MonthlyCostCurrency = "USD",
            VehicleRegNumber = null
        },
        new()
        {
            Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            PersonalNumber = "19700101-1111",
            Type = 2, // PersonalHealth
            MonthlyCostAmount = 20m,
            MonthlyCostCurrency = "USD",
            VehicleRegNumber = null
        }
    ];
}
