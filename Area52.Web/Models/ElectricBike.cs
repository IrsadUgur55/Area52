namespace Area52.Web.Models;

public class ElectricBike : Bike
{
    
    public int BatteryCapacityWh { get; set; }

    public ElectricBike()
    {
        Type = "Electric";
    }
}
