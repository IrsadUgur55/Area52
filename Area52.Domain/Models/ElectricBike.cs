namespace Area52.Domain.Models;

public class ElectricBike : Bike
{
    
    public int BatteryCapacityWh { get; set; }

    public ElectricBike()
    {
        Type = "Electric";
    }
}
