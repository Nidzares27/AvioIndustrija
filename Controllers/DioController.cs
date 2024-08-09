using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.ViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvioIndustrija.Controllers
{
    public class DioController : Controller
    {
        private readonly IDioRepository _dioRepository;

        public DioController(IDioRepository dioRepository)
        {
            this._dioRepository = dioRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Dio> djelovi = await _dioRepository.GetAll();
            return View(djelovi);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Dio dio)
        {
            IEnumerable<Dio> djelovi = await _dioRepository.GetAll();
            if (ModelState.IsValid)
            {
                string currentYear = DateTime.Now.Year.ToString();
                int currYear = Int32.Parse(currentYear);

                if (dio.GodinaProizvodnja < 1950 || dio.GodinaProizvodnja > currYear)
                {
                    ViewData["error"] = "Godina proizvodnje dijela ne smije biti manja od 1950 ili veca od trenutne godine";
                    return View(dio);
                }

                foreach (var item in djelovi)
                {
                    if (item.Naziv == dio.Naziv & item.Model == dio.Model)
                    {
                        ViewData["error"] = "Dio koji pokusavate da kreirate vec postoji.";
                        return View(dio);
                    }

                }
                _dioRepository.Add(dio);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje dijela nije uspjelo");
            }
            return View(dio);
        }
    }
}

