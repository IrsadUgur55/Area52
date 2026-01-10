namespace Area52.Domain.Options;

public class AssistanceOption : IRentalOption
{
    public string Name => "Assistance";

    public decimal CalculateExtra(decimal baseTotal, int bikeCount)
    {
        // €5 per gereserveerde fiets
        return 5m * bikeCount;
    }
}
