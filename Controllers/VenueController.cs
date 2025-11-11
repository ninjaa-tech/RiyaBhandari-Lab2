using Microsoft.AspNetCore.Mvc;
using RiyaBhandari_Lab2.Data;
using RiyaBhandari_Lab2.Models;

namespace RiyaBhandari_Lab2.Controllers
{
    public class VenueController : Controller
    {
        public IActionResult Index() => View(InMemoryData.Venues);

        public IActionResult Details(int id)
        {
            var venue = InMemoryData.FindVenue(id);
            if (venue == null) return NotFound();
            return View(venue);
        }
    }
}
