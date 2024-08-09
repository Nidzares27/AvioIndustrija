using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvioIndustrija.Controllers
{
    [Authorize]
    public class RelacijaController : Controller
    {
        private readonly IRelacijaRepository _relacijaRepository;

        public RelacijaController(IRelacijaRepository relacijaRepository)
        {
            this._relacijaRepository = relacijaRepository;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Relacija> relacije = await _relacijaRepository.GetAll();
            return View(relacije);
        }
        public async Task<IActionResult> Details(int id)
        {
            Relacija relacija = await _relacijaRepository.GetByIdAsyncNoTracking(id);
            return View(relacija);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Relacija relacija = await _relacijaRepository.GetByIdAsyncNoTracking(id);
            return View(relacija);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRelacija(int id)
        {
            Relacija relacija = await _relacijaRepository.GetByIdAsync(id);

            _relacijaRepository.Delete(relacija);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRelacijaViewModel relacijaVM)
        {
            IEnumerable<Relacija> relacije = await _relacijaRepository.GetAll();

            if (ModelState.IsValid)
            {

                var relacija = new Relacija
                {
                    GradOd = relacijaVM.GradOd,
                    DržavaOd = relacijaVM.DržavaOd,
                    AerodromOd = relacijaVM.AerodromOd,
                    GradDo = relacijaVM.GradDo,
                    DržavaDo = relacijaVM.DržavaDo,
                    AerodromDo = relacijaVM.AerodromDo,
                    UdaljenostKm = relacijaVM.UdaljenostKm,
                };

                foreach (var item in relacije)
                {
                    if ((item.AerodromOd == relacijaVM.AerodromOd & item.AerodromDo == relacijaVM.AerodromDo) & 
                        (item.GradOd == relacijaVM.GradOd & item.GradDo == relacijaVM.GradDo) &
                        (item.DržavaOd == relacijaVM.DržavaOd & item.DržavaDo == relacijaVM.DržavaDo))
                    {
                        ViewData["error"] = "Relacija koju pokusavate da kreirate vec postoji.";
                        return View(relacijaVM);
                    }
                }

                if (relacijaVM.GradOd == relacijaVM.GradDo || relacijaVM.DržavaOd == relacijaVM.DržavaDo || relacijaVM.AerodromOd == relacijaVM.AerodromDo)
                {
                    ViewData["error"] = "Relacija ne smije imati isti pocetni i krajnji (Grad, Drzavu ili Aerodrom)!";
                    return View(relacijaVM);
                }

                _relacijaRepository.Add(relacija);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje relacije nije uspjelo");
            }
            return View(relacijaVM);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Relacija relacija = await _relacijaRepository.GetByIdAsyncNoTracking(id);
            if (relacija == null) return View("Error");
            var relacijaVM = new EditRelacijaViewModel
            {
                GradOd = relacija.GradOd,
                DržavaOd = relacija.DržavaOd,
                AerodromOd = relacija.AerodromOd,
                GradDo = relacija.GradDo,
                DržavaDo = relacija.DržavaDo,
                AerodromDo = relacija.AerodromDo,
                UdaljenostKm = relacija.UdaljenostKm,
            };
            return View(relacijaVM);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRelacijaViewModel relacijaVM)
        {
            IEnumerable<Relacija> relacije = await _relacijaRepository.GetAllNoTracking();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje relacije nije uspjelo");
                return View("Edit", relacijaVM);
            }

            var relacija = await _relacijaRepository.GetByIdAsyncNoTracking(id);

            if (relacija != null)
            {
                var relacijaEdit = new Relacija
                {
                    RelacijaId = id,
                    GradOd = relacijaVM.GradOd,
                    DržavaOd = relacijaVM.DržavaOd,
                    AerodromOd = relacijaVM.AerodromOd,
                    GradDo = relacijaVM.GradDo,
                    DržavaDo = relacijaVM.DržavaDo,
                    AerodromDo = relacijaVM.AerodromDo,
                    UdaljenostKm = relacijaVM.UdaljenostKm,
                };

                if (relacijaVM.GradOd == relacijaVM.GradDo || relacijaVM.DržavaOd == relacijaVM.DržavaDo || relacijaVM.AerodromOd == relacijaVM.AerodromDo)
                {
                    ViewData["error"] = "Relacija ne smije imati isti pocetni i krajnji (Grad, Drzavu ili Aerodrom)!";
                    return View(relacijaVM);
                }

                foreach (var item in relacije)
                {
                    if ((item.AerodromOd == relacijaVM.AerodromOd & item.AerodromDo == relacijaVM.AerodromDo) &
                        (item.GradOd == relacijaVM.GradOd & item.GradDo == relacijaVM.GradDo) &
                        (item.DržavaOd == relacijaVM.DržavaOd & item.DržavaDo == relacijaVM.DržavaDo))
                    {
                        if ((item.AerodromOd == relacija.AerodromOd & item.AerodromDo == relacija.AerodromDo) &
                            (item.GradOd == relacija.GradOd & item.GradDo == relacija.GradDo) &
                            (item.DržavaOd == relacija.DržavaOd & item.DržavaDo == relacija.DržavaDo))
                        {
                            _relacijaRepository.Update(relacijaEdit);

                            return RedirectToAction("Index");
                        }

                        ViewData["error"] = "Relacija koju pokusavate da kreirate vec postoji.";
                        return View(relacijaVM);
                    }
                }


                _relacijaRepository.Update(relacijaEdit);

                return RedirectToAction("Index");
            }
            else
            {
                return View(relacijaVM);
            }
        }
    }
}
