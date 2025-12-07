using Microsoft.AspNetCore.Mvc;
using Area52.Web.Interfaces;
using Area52.Web.Models;

namespace Area52.Web.Controllers;

public class BikeController : Controller
{
    private readonly IBikeRepository _repo;

    public BikeController(IBikeRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var bikes = _repo.GetAll();
        return View(bikes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Bike bike)
    {
        if (!ModelState.IsValid)
        {
            return View(bike);
        }

        _repo.Add(bike);
        return RedirectToAction(nameof(Index));
    }

    // ---------- DETAILS ----------
    [HttpGet]
    public IActionResult Details(int id)
    {
        var bike = _repo.GetById(id);
        if (bike == null)
        {
            return NotFound();
        }

        return View(bike);
    }

    // ---------- EDIT ----------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var bike = _repo.GetById(id);
        if (bike == null)
        {
            return NotFound();
        }

        return View(bike);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Bike bike)
    {
        if (!ModelState.IsValid)
        {
            return View(bike);
        }

        _repo.Update(bike);
        return RedirectToAction(nameof(Index));
    }

    // ---------- DELETE ----------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var bike = _repo.GetById(id);
        if (bike == null)
        {
            return NotFound();
        }

        return View(bike);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _repo.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}

