using AvioIndustrija.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AvioIndustrija.Controllers
{
    public class AvionController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AvionController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Avion()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}