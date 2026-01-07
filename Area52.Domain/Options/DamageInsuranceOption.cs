namespace Area52.Domain.Options;

public class DamageInsuranceOption : IRentalOption
{
    public string Name => "Damage insurance";

    public decimal CalculateExtra(decimal baseTotal, int bikeCount)
    {
        // 2% van het totale verhuurbedrag
        return baseTotal * 0.02m;
    }
}
