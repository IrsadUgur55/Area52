using Area52.Domain.Interfaces;
using Area52.Domain.Models;

namespace Area52.Data.Repositories;

public class BikeReservationRepository : IBikeReservationRepository
{
    private static readonly List<BikeReservation> _reservations = new();
    private static int _nextId = 1;

    public IEnumerable<BikeReservation> GetAll()
    {
        return _reservations;
    }

    public BikeReservation? GetById(int id)
    {
        return _reservations.FirstOrDefault(r => r.ReservationId == id);
    }

    public void Add(BikeReservation reservation)
    {
        if (reservation == null) throw new ArgumentNullException(nameof(reservation));

        reservation.ReservationId = _nextId++;
        _reservations.Add(reservation);
    }

    public void Delete(int id)
    {
        var existing = GetById(id);
        if (existing != null)
        {
            _reservations.Remove(existing);
        }
    }
}
