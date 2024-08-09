using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace AvioIndustrija.Controllers
{
    [Authorize]
    public class LetController : Controller
    {
        private readonly ILetRepository _letRepository;
        private readonly IAvioniRepository _avioniRepository;
        private readonly IRelacijaRepository _relacijaRepository;

        public LetController(ILetRepository letRepository, IAvioniRepository avioniRepository, IRelacijaRepository relacijaRepository)
        {
            this._letRepository = letRepository;
            this._avioniRepository = avioniRepository;
            this._relacijaRepository = relacijaRepository;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Let> letovi = await _letRepository.GetAll();
            return View(letovi);
        }

        public async Task<IActionResult> Details(int id)
        {
            Let let = await _letRepository.GetByIdAsync(id);
            if (let == null) return View("Error");

            Avion avion = await _avioniRepository.GetByIdAsyncNoTracking(let.AvionId);
            Relacija relacija = await _relacijaRepository.GetByIdAsyncNoTracking(let.RelacijaId);
            var imeRelacije = relacija.AerodromOd + " -> " + relacija.AerodromDo;

            var letVM = new DetailsLetViewModel
            {
                LetId = id,
                BrojLeta = let.BrojLeta,
                ImeAviona = avion.ImeAviona,
                Relacija = imeRelacije,
                VrijemePoletanja = let.VrijemePoletanja,
                VrijemeSletanja = let.VrijemeSletanja,

            };
            return View(letVM);

            //Let let = await _letRepository.GetByIdAsyncNoTracking(id);
            //return View(let);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Let let = await _letRepository.GetByIdAsyncNoTracking(id);
            return View(let);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteLet(int id)
        {
            Let let = await _letRepository.GetByIdAsync(id);

            _letRepository.Delete(let);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Avion> avionii = await _avioniRepository.GetAll();
            IEnumerable<Relacija> relacijee = await _relacijaRepository.GetAll();

            ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
            ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");

            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLetViewModel letVM)
        {
            DateTime donjaGranicaPoletanja = new System.DateTime(
                1980, 1, 1, 0, 0, 0, 0);
            DateTime gornjaGranicaPoletanja = DateTime.Now;

            IEnumerable<Let> letovi = await _letRepository.GetByAvionAsync(letVM.AvionId);

            IEnumerable<Avion> avionii = await _avioniRepository.GetAll();
            IEnumerable<Relacija> relacijee = await _relacijaRepository.GetAll();

            if (ModelState.IsValid)
            {

                var let = new Let
                {
                    BrojLeta = letVM.BrojLeta,
                    AvionId = letVM.AvionId,
                    RelacijaId = letVM.RelacijaId,
                    VrijemePoletanja = letVM.VrijemePoletanja,
                    VrijemeSletanja = letVM.VrijemeSletanja,
                };

                if (letVM.VrijemePoletanja < donjaGranicaPoletanja || letVM.VrijemePoletanja > gornjaGranicaPoletanja)
                {
                    ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                    ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                    ViewData["error"] = "Vrijeme poletanja mora biti vece od 1.1.1980 i manje od trenutnog vremena!";
                    return View(letVM);
                }
                if (letVM.VrijemeSletanja < donjaGranicaPoletanja || letVM.VrijemeSletanja > gornjaGranicaPoletanja)
                {
                    ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                    ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                    ViewData["error"] = "Vrijeme sletanja mora biti vece od 1.1.1980 i manje od trenutnog vremena!";
                    return View(letVM);
                }
                if (letVM.VrijemePoletanja >= letVM.VrijemeSletanja)
                {
                    ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                    ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                    ViewData["error"] = "Vrijeme poletanja mora biti prije vremena sletanja!";
                    return View(letVM);
                }

                foreach (var item in letovi)
                {
                    if (letVM.VrijemePoletanja > item.VrijemePoletanja & letVM.VrijemePoletanja < item.VrijemeSletanja)
                    {
                        ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                        ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                        ViewData["error"] = "Avion ne moze letjeti 2 puta u isto vrijeme!";
                        return View(letVM);
                    }
                }

                _letRepository.Add(let);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje leta nije uspjelo");
            }
            return View(letVM);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Let let = await _letRepository.GetByIdAsyncNoTracking(id);
            if (let == null) return View("Error");

            IEnumerable<Avion> avionii = await _avioniRepository.GetAll();
            IEnumerable<Relacija> relacijee = await _relacijaRepository.GetAll();

            ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
            ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");

            var letVM = new EditLetViewModel
            {
                BrojLeta = let.BrojLeta,
                AvionId = let.AvionId,
                RelacijaId = let.RelacijaId,
                VrijemePoletanja = let.VrijemePoletanja,
                VrijemeSletanja = let.VrijemeSletanja,

            };
            return View(letVM);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditLetViewModel letVM)
        {
            DateTime donjaGranicaPoletanja = new System.DateTime(
            1980, 1, 1, 0, 0, 0, 0);
            DateTime gornjaGranicaPoletanja = DateTime.Now;

            IEnumerable<Let> letovi = await _letRepository.GetByAvionAsync(letVM.AvionId);

            IEnumerable<Avion> avionii = await _avioniRepository.GetAll();
            IEnumerable<Relacija> relacijee = await _relacijaRepository.GetAll();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje leta nije uspjelo");
                return View("Edit", letVM);
            }

            var let = await _letRepository.GetByIdAsyncNoTracking(id);

            if (let != null)
            {
                var letEdit = new Let
                {   
                    LetId = id,
                    BrojLeta = letVM.BrojLeta,
                    AvionId = letVM.AvionId,
                    RelacijaId = letVM.RelacijaId,
                    VrijemePoletanja = letVM.VrijemePoletanja,
                    VrijemeSletanja = letVM.VrijemeSletanja,
                };

                if ((letVM.VrijemePoletanja < donjaGranicaPoletanja || letVM.VrijemePoletanja > gornjaGranicaPoletanja) || (letVM.VrijemeSletanja < donjaGranicaPoletanja || letVM.VrijemeSletanja > gornjaGranicaPoletanja))
                {
                    ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                    ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                    ViewData["error"] = "Vrijeme Poletanja / Sletanja mora biti vece od 1.1.1980 i manje od trenutnog vremena!";
                    return View(letVM);
                }

                if (letVM.VrijemePoletanja >= letVM.VrijemeSletanja)
                {
                    ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                    ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                    ViewData["error"] = "Vrijeme poletanja mora biti prije vremena sletanja!";
                    return View(letVM);
                }

                foreach (var item in letovi)
                {
                    if (letVM.VrijemePoletanja > item.VrijemePoletanja & letVM.VrijemePoletanja < item.VrijemeSletanja)
                    {
                        ViewBag.AvionId = new SelectList(avionii, "AvionId", "AvionId");
                        ViewBag.RelacijaId = new SelectList(relacijee, "RelacijaId", "RelacijaId");
                        ViewData["error"] = "Avion ne moze letjeti 2 puta u isto vrijeme!";
                        return View(letVM);
                    }
                }

                _letRepository.Update(letEdit);

                return RedirectToAction("Index");
            }
            else
            {
                return View(letVM);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> IzvjestajOLetovima()
        {
            var viewModel = new IzvjestajOLetovimaViewModel { };
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IzvjestajOLetovima(IzvjestajOLetovimaViewModel izv)
        {
            DateTime datumOD = DateTime.Parse(izv.DatumOd);
            DateTime datumDO = DateTime.Parse(izv.DatumDo);

            if (datumDO < datumOD)
            {
                return View(izv);
            }

            IEnumerable<Let> letovi = await _letRepository.GetByDateRange(datumOD, datumDO);

            var letoviCount = letovi
                .GroupBy(i => i.AvionId)
                .Select(group => new { Item = group.Key, Count = group.Count() })
                .ToList();

            int maxCount = letoviCount.Max(x => x.Count);

            var mostFrequentItems = letoviCount
            .Where(x => x.Count == maxCount)
            .Select(x => new { x.Item, x.Count })
            .ToList();

            var najcesciAvioni = new List<Avion>();

            foreach (var item in mostFrequentItems)
            {
                var najcesciAvion = await _avioniRepository.GetByIdAsyncNoTracking(item.Item);
                najcesciAvioni.Add(najcesciAvion);
            }


            var relacijeCount = letovi
                .GroupBy(i => i.RelacijaId)
                .Select(group => new { Item = group.Key, Count = group.Count() })
                .ToList();

            int maxCountRelacija = relacijeCount.Max(x => x.Count);

            var mostFrequentRelacije = relacijeCount
            .Where(x => x.Count == maxCountRelacija)
            .Select(x => new { x.Item, x.Count })
            .ToList();

            var najcesceRelacije = new List<Relacija>();

            foreach (var item in mostFrequentRelacije)
            {
                var najcescaRelacija = await _relacijaRepository.GetByIdAsyncNoTracking(item.Item);
                najcesceRelacije.Add(najcescaRelacija);
            }

            var viewModel = new IzvjestajOLetovimaViewModel
            {
                letovi = letovi,
                brojLetova = letovi.Count(),
                brojLetovaNajcescihAviona = maxCount,
                najcesciAvioni = najcesciAvioni,
                brojLetovaNajcescimRelacijama = maxCountRelacija,
                najcesceRelacije = najcesceRelacije

            };

            return View(viewModel);
        }

    }
}
