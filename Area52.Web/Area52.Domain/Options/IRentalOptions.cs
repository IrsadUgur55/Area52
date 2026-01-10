namespace Area52.Domain.Options;

public interface IRentalOption
{
    string Name { get; }
    decimal CalculateExtra(decimal baseTotal, int bikeCount);
}
