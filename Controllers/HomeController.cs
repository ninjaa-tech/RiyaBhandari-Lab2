using Microsoft.AspNetCore.Mvc;
using RiyaBhandari_Lab2.Data;
using RiyaBhandari_Lab2.Models;
using System.Diagnostics;

namespace RiyaBhandari_Lab2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
    }
}