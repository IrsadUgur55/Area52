using Area52.Domain.Models;

namespace Area52.Domain.Interfaces;

public interface IBikeReservationRepository
{
    IEnumerable<BikeReservation> GetAll();
    BikeReservation? GetById(int id);
    void Add(BikeReservation reservation);
    void Delete(int id);
}
