namespace Area52.Web.Models
{
    public class Bike
    {
        public int BikeId { get; set; }
        public string Type { get; set; } // Electric, etc
        public decimal DayPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
