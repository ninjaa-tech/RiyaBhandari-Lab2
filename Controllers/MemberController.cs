using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiyaBhandari_Lab2.Data;
using RiyaBhandari_Lab2.Models;

namespace RiyaBhandari_Lab2.Controllers
{
    /// <summary>
    /// Full CRUD over in-memory Member data with validation.
    /// </summary>
    public class MemberController : Controller
    {
        // LIST
        public IActionResult Index() => View(InMemoryData.Members);

        // DETAILS
        public IActionResult Details(int id)
        {
            var member = InMemoryData.FindMember(id);
            if (member == null) return View("NotFound");
            member.MembershipPlan = member.MembershipPlanId.HasValue
                ? InMemoryData.FindPlan(member.MembershipPlanId.Value)
                : null;
            return View(member);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.PlanOptions = new SelectList(InMemoryData.Plans, "MembershipPlanId", "Name");
            return View(new Member());
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PlanOptions = new SelectList(InMemoryData.Plans, "MembershipPlanId", "Name", member.MembershipPlanId);
                return View(member);
            }

            InMemoryData.AddMember(member);   // <-- must exist in InMemoryData
            TempData["Success"] = "Member created.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var member = InMemoryData.FindMember(id);
            if (member == null) return View("NotFound");
            ViewBag.PlanOptions = new SelectList(InMemoryData.Plans, "MembershipPlanId", "Name", member.MembershipPlanId);
            return View(member);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Member member)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PlanOptions = new SelectList(InMemoryData.Plans, "MembershipPlanId", "Name", member.MembershipPlanId);
                return View(member);
            }

            if (!InMemoryData.UpdateMember(member)) return View("NotFound");
            TempData["Success"] = "Member updated.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)
        public IActionResult Delete(int id)
        {
            var member = InMemoryData.FindMember(id);
            if (member == null) return View("NotFound");
            return View(member);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!InMemoryData.DeleteMember(id)) return View("NotFound");
            TempData["Success"] = "Member deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
