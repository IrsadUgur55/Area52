using Area52.Domain.Models;
using Area52.Domain.Options;

namespace Area52.Domain.Services;

public interface IBikePricingService
{
    decimal CalculateTotal(Bike bike, int days, int bikeCount, IEnumerable<IRentalOption> options);
}
