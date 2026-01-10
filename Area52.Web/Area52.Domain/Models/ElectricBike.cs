namespace Area52.Domain.Models;

public class ElectricBike : Bike
{
    
    public int BatteryCapacityWh { get; set; }

    public ElectricBike()
    {
        // BELANGRIJK: Type moet exact "ElectricBike" zijn voor database mapping!
        // De BikeRepository.MapToBike() methode checkt op deze exacte string.
        Type = "ElectricBike";
    }
    public override decimal GetDailySurcharge()
{
    // FR-01: diefstalverzekering op basis van leeftijd van de fiets
    var ageInDays = (DateTime.Today - PurchaseDate).TotalDays;

    decimal percentage =
        ageInDays < 365 ? 0.10m :
        ageInDays < 730 ? 0.05m :
        0.025m;

    return DayPrice * percentage;
}
}


