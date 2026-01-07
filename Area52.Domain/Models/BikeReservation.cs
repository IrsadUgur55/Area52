using Area52.Domain.Options;

namespace Area52.Domain.Models;

public class BikeReservation
{
    public int ReservationId { get; set; } // handig voor later (DB/in-memory)
    public int BikeId { get; set; }

    public DateTime StartDate { get; set; }
    public int Days { get; set; } = 1;

    // Voor nu 1 fiets per reservering
    public int BikeCount { get; set; } = 1;

    // Gekozen opties (FR-02)
    public List<IRentalOption> Options { get; set; } = new();

    // Totaalprijs die berekend wordt via BikePricingService
    public decimal TotalPrice { get; set; }
}
