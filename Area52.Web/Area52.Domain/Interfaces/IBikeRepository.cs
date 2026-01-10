using Area52.Domain.Models;

namespace Area52.Domain.Interfaces;

public interface IBikeRepository
{
    IEnumerable<Bike> GetAll();
    Bike? GetById(int id);
    void Add(Bike bike);
    void Update(Bike bike);
    void Delete(int id);
}
