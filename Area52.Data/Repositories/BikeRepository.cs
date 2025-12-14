using Area52.Domain.Interfaces;
using Area52.Domain.Models;

namespace Area52.Data.Repositories;

public class BikeRepository : IBikeRepository
{
    private static readonly List<Bike> _bikes = new()
{
    new CityBike
    {
        BikeId = 1,
        DayPrice = 10.0m,
        PurchaseDate = new DateTime(2022, 1, 1)
    },
    new ElectricBike
    {
        BikeId = 2,
        DayPrice = 25.0m,
        PurchaseDate = new DateTime(2023, 6, 15),
        BatteryCapacityWh = 500
    }
};

    public IEnumerable<Bike> GetAll() => _bikes;

    public Bike? GetById(int id)
        => _bikes.FirstOrDefault(b => b.BikeId == id);

    public void Add(Bike bike)
    {
        var nextId = _bikes.Any() ? _bikes.Max(b => b.BikeId) + 1 : 1;
        bike.BikeId = nextId;
        _bikes.Add(bike);
    }

    public void Update(Bike bike)
    {
        var existing = _bikes.FirstOrDefault(b => b.BikeId == bike.BikeId);
        if (existing is null) return;

        existing.Type = bike.Type;
        existing.DayPrice = bike.DayPrice;
        existing.PurchaseDate = bike.PurchaseDate;
    }

    public void Delete(int id)
    {
        var existing = _bikes.FirstOrDefault(b => b.BikeId == id);
        if (existing is not null)
        {
            _bikes.Remove(existing);
        }
    }
}
