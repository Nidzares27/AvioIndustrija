using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Controllers
{
    [Authorize]
    public class IstorijaLetovaPutnikaController : Controller
    {
        private readonly AvioIndustrija2424Context _context;
        private readonly IIstorijaLetovaPutnikaRepository _istorijaLetovaPutnikaRepository;
        private readonly ILetRepository _letRepository;
        private readonly IPutnikRepository _putnikRepository;
        private readonly IAvioniRepository _avioniRepository;

        public IstorijaLetovaPutnikaController(AvioIndustrija2424Context context, IIstorijaLetovaPutnikaRepository istorijaLetovaPutnikaRepository, ILetRepository letRepository, IPutnikRepository putnikRepository, IAvioniRepository avioniRepository)
        {
            this._context = context;
            this._istorijaLetovaPutnikaRepository = istorijaLetovaPutnikaRepository;
            this._letRepository = letRepository;
            this._putnikRepository = putnikRepository;
            this._avioniRepository = avioniRepository;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LetSortParm"] = String.IsNullOrEmpty(sortOrder) ? "let_desc" : "";
            ViewData["KlasaSortParm"] = sortOrder == "Klasa" ? "klasa_desc" : "Klasa";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var istorija = from i in _context.IstorijaLetovaPutnikas
                         select i;


            if (!String.IsNullOrEmpty(searchString))
            {
                var ss = Int32.Parse(searchString);

                istorija = istorija.Where(i => i.PutnikId.Equals(ss)
                                       || i.LetId.Equals(ss));
            }
            switch (sortOrder)
            {
                case "let_desc":
                    istorija = istorija.OrderByDescending(i => i.LetId);
                    break;
                case "Klasa":
                    istorija = istorija.OrderBy(i => i.Klasa);
                    break;
                case "klasa_desc":
                    istorija = istorija.OrderByDescending(i => i.Klasa);
                    break;
                default:
                    istorija = istorija.OrderBy(i => i.LetId);
                    break;
            }

            int pageSize = 8;
            return View(await PaginatedList<IstorijaLetovaPutnika>.CreateAsync(istorija.AsNoTracking(), pageNumber ?? 1, pageSize));

            //IEnumerable<IstorijaLetovaPutnika> istorijaLetovaPutnika = await _istorijaLetovaPutnikaRepository.GetAll();
            //return View(istorijaLetovaPutnika);
        }

        public async Task<IActionResult> Details(int putnikID, int letID )
        {
            var p = ((object[])RouteData.Values.Values)[2].ToString();
            putnikID = Int32.Parse(p);
            var l = ((object[])RouteData.Values.Values)[3].ToString();
            letID = Int32.Parse(l);

            IstorijaLetovaPutnika istorijaLetovaPutnika = await _istorijaLetovaPutnikaRepository.GetByIdAsyncNoTracking(putnikID, letID);

            Let let = await _letRepository.GetByIdAsync(istorijaLetovaPutnika.LetId);
            Putnik putnik = await _putnikRepository.GetByIdAsyncNoTracking(istorijaLetovaPutnika.PutnikId);
            var imePrezime = putnik.Ime + " " + putnik.Prezime;

            var ilp = new DetailsIstorijaLetovaPutnikViewModel
            {
                ImePutnika = imePrezime,
                BrojLeta = let.BrojLeta,
                RedniBrojSjedišta = istorijaLetovaPutnika.RedniBrojSjedišta,
                Klasa = istorijaLetovaPutnika.Klasa,
                RučniPrtljag8kg = istorijaLetovaPutnika.RučniPrtljag8kg,
                KoferiKg = istorijaLetovaPutnika.KoferiKg,
            };

            return View(ilp);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int putnikID, int letID)
        {
            var p = ((object[])RouteData.Values.Values)[2].ToString();
            putnikID = Int32.Parse(p);
            var l = ((object[])RouteData.Values.Values)[3].ToString();
            letID = Int32.Parse(l);

            IstorijaLetovaPutnika istorijaLetovaPutnika = await _istorijaLetovaPutnikaRepository.GetByIdAsyncNoTracking(putnikID, letID);
            return View(istorijaLetovaPutnika);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteIstorijaLetovaPutnika(int putnikID, int letID)
        {
            var p = ((object[])RouteData.Values.Values)[2].ToString();
            putnikID = Int32.Parse(p);
            var l = ((object[])RouteData.Values.Values)[3].ToString();
            letID = Int32.Parse(l);

            IstorijaLetovaPutnika istorijaLetovaPutnika = await _istorijaLetovaPutnikaRepository.GetByIdAsync(putnikID, letID);

            _istorijaLetovaPutnikaRepository.Delete(istorijaLetovaPutnika);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Let> letovi = await _letRepository.GetAll();
            IEnumerable<Putnik> putnici = await _putnikRepository.GetAll();

            ViewBag.LetId = new SelectList(letovi, "LetId", "LetId");
            ViewBag.PutnikId = new SelectList(putnici, "PutnikId", "PutnikId");

            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateIstorijaLetovaPutnikaViewModel ilpVM)
        {
            IEnumerable<IstorijaLetovaPutnika> let = await _istorijaLetovaPutnikaRepository.GetByLetAsync(ilpVM.LetId);

            IEnumerable<Let> letovi = await _letRepository.GetAll();
            IEnumerable<Putnik> putnici = await _putnikRepository.GetAll();

            if (ModelState.IsValid)
            {

                var ilp = new IstorijaLetovaPutnika
                {
                    PutnikId = ilpVM.PutnikId,
                    LetId = ilpVM.LetId,
                    RedniBrojSjedišta = ilpVM.RedniBrojSjedišta,
                    Klasa = ilpVM.Klasa,
                    RučniPrtljag8kg = ilpVM.RučniPrtljag8kg,
                    KoferiKg = ilpVM.KoferiKg,
                };

                var lett = await _letRepository.GetByIdAsyncNoTracking(ilpVM.LetId);
                var avion = await _avioniRepository.GetByIdAsyncNoTracking(lett.AvionId);
                var ukupanBrSjedista = avion.BrojSjedištaBiznisKlase + avion.BrojSjedištaEkonomskeKlase;

                if (ilpVM.RedniBrojSjedišta > ukupanBrSjedista)
                {
                    ViewBag.LetId = new SelectList(letovi, "LetId", "LetId");
                    ViewBag.PutnikId = new SelectList(putnici, "PutnikId", "PutnikId");
                    ViewData["error"] = "Avion za ovaj let nema sjediste sa datim rednim brojem!";
                    return View(ilpVM);
                }

                foreach (var item in let)
                {
                    if (item.RedniBrojSjedišta == ilpVM.RedniBrojSjedišta)
                    {
                        ViewBag.LetId = new SelectList(letovi, "LetId", "LetId");
                        ViewBag.PutnikId = new SelectList(putnici, "PutnikId", "PutnikId");
                        ViewData["error"] = "Vec postoji putnik za dato sjediste";
                        return View(ilpVM);
                    }
                }

                _istorijaLetovaPutnikaRepository.Add(ilp);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje istorije leta putnika nije uspjelo");
            }
            return View(ilpVM);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int putnikID, int letID)
        {
            var p = ((object[])RouteData.Values.Values)[2].ToString();
            putnikID = Int32.Parse(p);
            var l = ((object[])RouteData.Values.Values)[3].ToString();
            letID = Int32.Parse(l);

            IstorijaLetovaPutnika istorijaLetovaPutnika = await _istorijaLetovaPutnikaRepository.GetByIdAsyncNoTracking(putnikID, letID);
            if (istorijaLetovaPutnika == null) return View("Error");

            IEnumerable<Let> letovi = await _letRepository.GetAll();
            IEnumerable<Putnik> putnici = await _putnikRepository.GetAll();

            ViewBag.LetId = new SelectList(letovi, "LetId", "LetId");
            ViewBag.PutnikId = new SelectList(putnici, "PutnikId", "PutnikId");


            var ilpVM = new EditIstorijaLetovaPutnikaViewModel
            {
                PutnikId = istorijaLetovaPutnika.PutnikId,
                LetId = istorijaLetovaPutnika.LetId,
                RedniBrojSjedišta = istorijaLetovaPutnika.RedniBrojSjedišta,
                Klasa = istorijaLetovaPutnika.Klasa,
                RučniPrtljag8kg = istorijaLetovaPutnika.RučniPrtljag8kg,
                KoferiKg = istorijaLetovaPutnika.KoferiKg,

            };
            return View(ilpVM);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int putnikID, int letID, EditIstorijaLetovaPutnikaViewModel ilpVM)
        {
            IEnumerable<IstorijaLetovaPutnika> let = await _istorijaLetovaPutnikaRepository.GetByLetAsync(ilpVM.LetId);

            IEnumerable<Let> letovi = await _letRepository.GetAll();
            IEnumerable<Putnik> putnici = await _putnikRepository.GetAll();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje istorije leta putnika nije uspjelo");
                return View("Edit", ilpVM);
            }

            var p = ((object[])RouteData.Values.Values)[2].ToString();
            putnikID = Int32.Parse(p);
            var l = ((object[])RouteData.Values.Values)[3].ToString();
            letID = Int32.Parse(l);

            var istorijaLetovaPutnika = await _istorijaLetovaPutnikaRepository.GetByIdAsyncNoTracking(putnikID, letID);

            var lett = await _letRepository.GetByIdAsyncNoTracking(istorijaLetovaPutnika.LetId);
            var avion = await _avioniRepository.GetByIdAsyncNoTracking(lett.AvionId);
            var ukupanBrSjedista = avion.BrojSjedištaBiznisKlase + avion.BrojSjedištaEkonomskeKlase;

            if (istorijaLetovaPutnika != null)
            {
                var ilpEdit = new IstorijaLetovaPutnika
                {
                    PutnikId = putnikID,
                    LetId = letID,
                    RedniBrojSjedišta = ilpVM.RedniBrojSjedišta,
                    Klasa = ilpVM.Klasa,
                    RučniPrtljag8kg = ilpVM.RučniPrtljag8kg,
                    KoferiKg = ilpVM.KoferiKg,
                };

                if (ilpVM.RedniBrojSjedišta > ukupanBrSjedista)
                {
                    ViewBag.LetId = new SelectList(letovi, "LetId", "LetId");
                    ViewBag.PutnikId = new SelectList(putnici, "PutnikId", "PutnikId");
                    ViewData["error"] = "Avion za ovaj let nema sjediste sa datim rednim brojem!";
                    return View(ilpVM);
                }

                foreach (var item in let)
                {
                    if (item.RedniBrojSjedišta == ilpVM.RedniBrojSjedišta)
                    {
                        if (item.RedniBrojSjedišta == istorijaLetovaPutnika.RedniBrojSjedišta)
                        {
                            _istorijaLetovaPutnikaRepository.Update(ilpEdit);

                            return RedirectToAction("Index");
                        }
                        ViewBag.LetId = new SelectList(letovi, "LetId", "LetId");
                        ViewBag.PutnikId = new SelectList(putnici, "PutnikId", "PutnikId");
                        ViewData["error"] = "Vec postoji putnik za dato sjediste";
                        return View(ilpVM);
                    }
                }

                _istorijaLetovaPutnikaRepository.Update(ilpEdit);

                return RedirectToAction("Index");
            }
            else
            {
                return View(ilpVM);
            }
        }
    }
}
