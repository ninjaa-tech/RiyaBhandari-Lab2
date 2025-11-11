using Microsoft.AspNetCore.Mvc;
using RiyaBhandari_Lab2.Data;

namespace RiyaBhandari_Lab2.Controllers
{
    public class MembershipPlanController : Controller
    {
        public IActionResult Index() => View(InMemoryData.Plans);
    }
}
