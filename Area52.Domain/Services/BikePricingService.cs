using Area52.Domain.Models;
using Area52.Domain.Options;

namespace Area52.Domain.Services;

public class BikePricingService : IBikePricingService
{
    public decimal CalculateTotal(Bike bike, int days, int bikeCount, IEnumerable<IRentalOption> options)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days), "Days must be > 0.");
        if (bikeCount <= 0) throw new ArgumentOutOfRangeException(nameof(bikeCount), "BikeCount must be > 0.");
        if (bike is null) throw new ArgumentNullException(nameof(bike));

        var dailyTotal = bike.DayPrice + bike.GetDailySurcharge();
        var baseTotal = dailyTotal * days * bikeCount;


        var optionsExtra = (options ?? Enumerable.Empty<IRentalOption>())
            .Sum(opt => opt.CalculateExtra(baseTotal, bikeCount));

        return baseTotal + optionsExtra;
    }
}
