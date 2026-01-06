using Microsoft.AspNetCore.Mvc;
using Area52.Domain.Interfaces;
using Area52.Domain.Models;
using Area52.Domain.Options;
using Area52.Domain.Services;

namespace Area52.Web.Controllers;

public class ReservationController : Controller
{
    private readonly IBikeRepository _bikeRepo;
    private readonly IBikeReservationRepository _reservationRepo;
    private readonly IBikePricingService _pricingService;

    public ReservationController(
        IBikeRepository bikeRepo,
        IBikeReservationRepository reservationRepo,
        IBikePricingService pricingService)
    {
        _bikeRepo = bikeRepo;
        _reservationRepo = reservationRepo;
        _pricingService = pricingService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var bikes = _bikeRepo.GetAll();
        return View(bikes);
    }

    [HttpGet]
    public IActionResult Index()
    {
    var reservations = _reservationRepo.GetAll();

    var items = reservations.Select(r =>
    {
        var bike = _bikeRepo.GetById(r.BikeId);

        return new Area52.Web.Models.ReservationOverviewItem
        {
            ReservationId = r.ReservationId,
            BikeId = r.BikeId,
            BikeType = bike?.Type ?? "Unknown",
            StartDate = r.StartDate,
            Days = r.Days,
            TotalPrice = r.TotalPrice,
            OptionsText = (r.Options != null && r.Options.Any())
                ? string.Join(", ", r.Options.Select(o => o.Name))
                : "-"
        };
    }).ToList();

    return View(items);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
    var reservation = _reservationRepo.GetById(id);
    if (reservation == null) return NotFound();

    var bike = _bikeRepo.GetById(reservation.BikeId);
    if (bike == null) return NotFound();

    ViewBag.BikeType = bike.Type;
    ViewBag.DayPrice = bike.DayPrice;

    return View(reservation);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
    var reservation = _reservationRepo.GetById(id);
    if (reservation == null) return NotFound();

    var bike = _bikeRepo.GetById(reservation.BikeId);
    if (bike == null) return NotFound();

    ViewBag.BikeType = bike.Type;

    return View(reservation);
    }


[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int id)
{
    _reservationRepo.Delete(id);
    return RedirectToAction(nameof(Index));
}




    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(int bikeId, DateTime startDate, int days, string[]? optionKeys)
    {
        if (days <= 0)
        {
            ModelState.AddModelError(nameof(days), "Days must be at least 1.");
        }

        var bike = _bikeRepo.GetById(bikeId);
        if (bike == null) return NotFound();

        if (!ModelState.IsValid)
        {
            var bikes = _bikeRepo.GetAll();
            return View(bikes);
        }

        // ✅ Keys -> IRentalOption instances
        var options = new List<IRentalOption>();
        foreach (var key in optionKeys ?? Array.Empty<string>())
        {
            switch (key)
            {
                case "DamageInsurance":
                    options.Add(new DamageInsuranceOption());
                    break;
                case "Assistance":
                    options.Add(new AssistanceOption());
                    break;
            }
        }

        var total = _pricingService.CalculateTotal(bike, days, bikeCount: 1, options);

        var reservation = new BikeReservation
        {
            BikeId = bike.BikeId,
            StartDate = startDate,
            Days = days,
            BikeCount = 1,
            TotalPrice = total,
            Options = options
        };

        _reservationRepo.Add(reservation);

        return RedirectToAction(nameof(Confirmation), new { id = reservation.ReservationId });
    }

    [HttpGet]
    public IActionResult Confirmation(int id)
    {
        var reservation = _reservationRepo.GetById(id);
        if (reservation == null) return NotFound();

        var bike = _bikeRepo.GetById(reservation.BikeId);
        if (bike == null) return NotFound();

        ViewBag.BikeType = bike.Type;
        ViewBag.DayPrice = bike.DayPrice;

        return View(reservation);
    }
}
