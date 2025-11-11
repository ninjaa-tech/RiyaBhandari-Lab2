using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiyaBhandari_Lab2.Data;
using RiyaBhandari_Lab2.Models;
using RiyaBhandari_Lab2.ViewModels;

namespace RiyaBhandari_Lab2.Controllers
{
    /// <summary>
    /// Booking list + CRUD using a form ViewModel for dropdowns/checkboxes.
    /// </summary>
    public class BookingController : Controller
    {
        // GET: /Booking
        public IActionResult Index()
        {
            foreach (var b in InMemoryData.Bookings)
            {
                b.Venue ??= InMemoryData.FindVenue(b.VenueId);
                b.Member ??= b.MemberId.HasValue ? InMemoryData.FindMember(b.MemberId.Value) : null;
            }
            var model = InMemoryData.Bookings.OrderBy(b => b.Date);
            return View(model);
        }

        // GET: /Booking/Details/3
        public IActionResult Details(int id)
        {
            var booking = InMemoryData.FindBooking(id);
            if (booking == null) return View("NotFound");
            booking.Venue ??= InMemoryData.FindVenue(booking.VenueId);
            booking.Member ??= booking.MemberId.HasValue ? InMemoryData.FindMember(booking.MemberId.Value) : null;
            return View(booking);
        }

        // GET: /Booking/Create
        public IActionResult Create()
        {
            return View(BuildVM(new Booking
            {
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = "10:00–12:00",
                TotalPrice = 100m
            }));
        }

        // POST: /Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingFormVM vm)
        {
            if (!ModelState.IsValid)
                return View(BuildVM(vm.Booking));

            // Map selected AddOns
            vm.Booking.AddOns = vm.SelectedAddOnIds
                .Select(id => new BookingAddOn { AddOnId = id })
                .ToList();

            InMemoryData.AddBooking(vm.Booking);
            TempData["Success"] = "Booking created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Booking/Edit/5
        public IActionResult Edit(int id)
        {
            var b = InMemoryData.FindBooking(id);
            if (b == null) return View("NotFound");

            var vm = BuildVM(b);
            vm.SelectedAddOnIds = b.AddOns.Select(a => a.AddOnId).ToList();
            return View(vm);
        }

        // POST: /Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookingFormVM vm)
        {
            if (!ModelState.IsValid)
                return View(BuildVM(vm.Booking));

            vm.Booking.AddOns = vm.SelectedAddOnIds
                .Select(id => new BookingAddOn { BookingId = vm.Booking.BookingId, AddOnId = id })
                .ToList();

            if (!InMemoryData.UpdateBooking(vm.Booking)) return View("NotFound");
            TempData["Success"] = "Booking updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Booking/Delete/5
        public IActionResult Delete(int id)
        {
            var b = InMemoryData.FindBooking(id);
            if (b == null) return View("NotFound");
            b.Venue ??= InMemoryData.FindVenue(b.VenueId);
            b.Member ??= b.MemberId.HasValue ? InMemoryData.FindMember(b.MemberId.Value) : null;
            return View(b);
        }

        // POST: /Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!InMemoryData.DeleteBooking(id)) return View("NotFound");
            TempData["Success"] = "Booking deleted.";
            return RedirectToAction(nameof(Index));
        }

        // helper to build dropdown/checkbox options
        private static BookingFormVM BuildVM(Booking b) => new BookingFormVM
        {
            Booking = b,
            VenueOptions = InMemoryData.Venues
                .Select(v => new SelectListItem { Value = v.VenueId.ToString(), Text = v.Name }),
            MemberOptions = InMemoryData.Members
                .Select(m => new SelectListItem { Value = m.MemberId.ToString(), Text = m.FullName }),
            AddOnOptions = InMemoryData.AddOns
                .Select(a => new SelectListItem { Value = a.AddOnId.ToString(), Text = $"{a.Name} ($ {a.Cost})" })
        };
    }
}
