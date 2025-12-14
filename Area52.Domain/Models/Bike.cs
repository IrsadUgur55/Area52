namespace Area52.Domain.Models;

public class Bike
{
    public int BikeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal DayPrice { get; set; }
    public DateTime PurchaseDate { get; set; }
}
