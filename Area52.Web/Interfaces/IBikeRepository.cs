using Area52.Web.Models;

namespace Area52.Web.Interfaces;

public interface IBikeRepository
{
    IEnumerable<Bike> GetAll();
    Bike? GetById(int id);
    void Add(Bike bike);
    void Update(Bike bike);
    void Delete(int id);
}
