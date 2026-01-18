using Xunit;
using Area52.Domain.Models;
using Area52.Domain.Services;

namespace Area52.Tests;

public class BikePricingServiceTests
{
    [Fact]
    public void CalculateTotal_WithValidInput_ReturnsExpectedTotal()
    {
        // Arrange
        var service = new BikePricingService();
        var bike = new CityBike { DayPrice = 10m };
        int days = 2;
        int bikeCount = 1;

        // Act
        var total = service.CalculateTotal(bike, days, bikeCount, options: null);

        // Assert
        Assert.Equal(20m, total);
    }

    [Fact]
    public void CalculateTotal_WithDaysZero_ThrowsArgumentOutOfRangeException()
    {
        var service = new BikePricingService();
        var bike = new CityBike { DayPrice = 10m };

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            service.CalculateTotal(bike, days: 0, bikeCount: 1, options: null)
        );
    }

    [Fact]
    public void CalculateTotal_WithNullBike_ThrowsArgumentNullException()
    {
        var service = new BikePricingService();

        Assert.Throws<ArgumentNullException>(() =>
            service.CalculateTotal(bike: null!, days: 1, bikeCount: 1, options: null)
        );
    }
}
