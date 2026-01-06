namespace Area52.Web.Models;

public class ReservationOverviewItem
{
    public int ReservationId { get; set; }
    public int BikeId { get; set; }
    public string BikeType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public int Days { get; set; }
    public decimal TotalPrice { get; set; }
    public string OptionsText { get; set; } = string.Empty;
}
