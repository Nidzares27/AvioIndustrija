using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.Services;
using AvioIndustrija.Utils;
using AvioIndustrija.ViewModels;
using AvioIndustrija.ViewModels.Komentar;
using AvioIndustrija.ViewModels.Vijest;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using DocumentFormat.OpenXml.Wordprocessing;
using Markdig;

namespace AvioIndustrija.Controllers
{
    [Authorize]
    public class VijestController : Controller
    {
        private readonly AvioIndustrija2424Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IVijestRepository _vijestRepository;
        private readonly IOcjenaRepository _ocjenaRepository;
        private readonly IKomentarRepository _komentarRepository;
        private readonly MarkdownService _markdownService;

        public VijestController(AvioIndustrija2424Context context, UserManager<AppUser> userManager, IVijestRepository vijestRepository, IOcjenaRepository ocjenaRepository, IKomentarRepository komentarRepository, MarkdownService markdownService)
        {
            this._context = context;
            this._userManager = userManager;
            this._vijestRepository = vijestRepository;
            this._ocjenaRepository = ocjenaRepository;
            this._komentarRepository = komentarRepository;
            this._markdownService = markdownService;
        }


        public async Task<IActionResult> Index()
        {

            var newsList = await _context.Vijests.ToListAsync();
            var vijesti = new List<VijestViewModel>();
            foreach (var item in newsList)
            {
                var autor = await _context.AppUsers.FirstOrDefaultAsync(b => b.Id == item.KorisnikId);
                var vijest = new VijestViewModel();
                vijest.VijestId = item.VijestId;
                vijest.Naslov = item.Naslov;
                vijest.Sadrzaj = item.Sadrzaj;
                vijest.VrijemeObjave = item.VrijemeObjave;
                vijest.ImageUrl = item.ImageUrl;
                vijest.BrojPregleda = item.BrojPregleda;
                vijest.ProsjecnaOcjena = item.ProsjecnaOcjena;
                vijest.ImeAutora = autor.Ime;
                vijest.PrezimeAutora = autor.Prezime;

                vijesti.Add(vijest);
            }
            return View(vijesti);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateVijestViewModel createVijestViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                createVijestViewModel.KorisnikId = user.Id;
                createVijestViewModel.VrijemeObjave = DateTime.Now;
                createVijestViewModel.ProsjecnaOcjena = 0;
                createVijestViewModel.BrojPregleda = 0;

                var vijest = new Vijest();
                vijest.Naslov = createVijestViewModel.Naslov;
                vijest.Sadrzaj = createVijestViewModel.Sadrzaj;
                vijest.VrijemeObjave = createVijestViewModel.VrijemeObjave;
                vijest.KorisnikId = createVijestViewModel.KorisnikId;
                vijest.ImageUrl = createVijestViewModel.ImageUrl;
                vijest.BrojPregleda = createVijestViewModel.BrojPregleda;
                vijest.ProsjecnaOcjena = createVijestViewModel.ProsjecnaOcjena;

                _vijestRepository.Add(vijest);
                return RedirectToAction(nameof(Index));
            }
            return View(createVijestViewModel);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Vijest vijest = await _vijestRepository.GetByIdAsyncNoTracking(id);
            if (vijest == null) return View("Error");
            var vijestVM = new EditVijestViewModel
            {
                VijestId = vijest.VijestId,
                Naslov = vijest.Naslov,
                Sadrzaj = vijest.Sadrzaj,
                VrijemeObjave = vijest.VrijemeObjave,
                ImageUrl = vijest.ImageUrl,
                BrojPregleda = vijest.BrojPregleda,
                ProsjecnaOcjena = vijest.ProsjecnaOcjena,
                KorisnikId = vijest.KorisnikId
            };
            return View(vijestVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditVijestViewModel vijestVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje vijesti nije uspjelo");
                return View("Edit", vijestVM);
            }

            var vijest = await _vijestRepository.GetByIdAsyncNoTracking(id);

            if (vijest == null)
            {
                return View(vijestVM);
            }

            var vijestEdit = new Vijest
            {
                VijestId = id,
                Naslov = vijestVM.Naslov,
                Sadrzaj = vijestVM.Sadrzaj,
                VrijemeObjave = vijestVM.VrijemeObjave,
                ImageUrl = vijestVM.ImageUrl,
                BrojPregleda = vijestVM.BrojPregleda,
                ProsjecnaOcjena = vijestVM.ProsjecnaOcjena,
                KorisnikId = vijestVM.KorisnikId,
            };

            _vijestRepository.Update(vijestEdit);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var vijesti = await _vijestRepository.GetAll();
            var listaVijesti = new List<Vijest>();
            var b = 1;
            foreach (var item in vijesti.Where(c => c.VijestId != id).OrderBy(b => b.VrijemeObjave))
            {
                if (b <= 7)
                {
                    listaVijesti.Add(item);
                    b++;
                }
                else
                {
                    break;
                }
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;

            

            var cookieKey = $"news_visited_{userId}_{id}";

            var existingRating = _context.Ocjenas.FirstOrDefault(r => r.VijestId == id && r.KorisnikId == userId);

            var news = await _context.Vijests
                .Include(n => n.Komentars)
                .Include(n => n.Ocjenas)
                .FirstOrDefaultAsync(n => n.VijestId == id);

            if (news == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "user"))
            {
                if (!Request.Cookies.TryGetValue(cookieKey, out string d))
                {
                    news.BrojPregleda++;
                    _context.Update(news);
                    await _context.SaveChangesAsync();

                    Response.Cookies.Append(cookieKey, "true", new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddYears(1) // Adjust the expiration as needed
                    });
                }
            }

            var comments = news.Komentars
                .OrderBy(c => c.Vrijeme)
                .ToList();

            var structuredComments = comments
                .Where(c => c.NadKomentar == null)
                .Select(c => new KomentarVijestiViewModel
                {
                    KomentarId = c.KomentarId,
                    Sadrzaj = c.Sadrzaj,
                    Vrijeme = c.Vrijeme,
                    VijestId = c.VijestId,
                    KorisnikId = c.KorisnikId,
                    Replies = GetReplies(c.KomentarId, comments, id).Result
                })
                .ToList();

            var i = 0;
            foreach (var item in news.Komentars.Where(c => c.NadKomentar == null))
            {
                var korisnikk = await _context.AppUsers.FirstOrDefaultAsync(b => b.Id == item.KorisnikId);
                structuredComments[i].ImeKorisnika = korisnikk.Ime;
                structuredComments[i].PrezimeKorisnika = korisnikk.Prezime;
                i++;
            }

            var ocjenaKorisnika = await _context.Ocjenas.FirstOrDefaultAsync(b => b.KorisnikId == news.KorisnikId);
            var korisnik = await _context.AppUsers.FirstOrDefaultAsync(b => b.Id == news.KorisnikId);

            var vijest = new DetailsVijestViewModel();
            vijest.VijestId = news.VijestId;
            vijest.VrijemeObjave = news.VrijemeObjave;
            vijest.Sadrzaj = news.Sadrzaj;
            vijest.ProsjecnaOcjena = news.ProsjecnaOcjena;
            vijest.Naslov = news.Naslov;
            vijest.KorisnikId = news.KorisnikId;
            vijest.ImageUrl = news.ImageUrl;
            vijest.BrojPregleda = news.BrojPregleda;
            vijest.Komentari = structuredComments;
            if (ocjenaKorisnika != null)
            {
                vijest.Ocjena = ocjenaKorisnika.Zvijezda;
            }
            vijest.ImeKorisnika = korisnik.Ime;
            vijest.PrezimeKorisnika = korisnik.Prezime;
            vijest.UserRating = existingRating?.Zvijezda ?? 0;
            vijest.ListaPredloga = listaVijesti;

            ///
            var html = _markdownService.ConvertToHtml(vijest.Sadrzaj);
            ViewBag.HtmlContent = html;
            ///
            return View(vijest);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(KomentarViewModel komentarViewModel)
        {     
            var user = await _userManager.GetUserAsync(User);
            var comment = new Komentar
            {
                Sadrzaj = komentarViewModel.Sadrzaj,
                Vrijeme = DateTime.Now,
                KorisnikId = user.Id,
                VijestId = komentarViewModel.VijestId,
                NadKomentar = komentarViewModel.NadKomentar ?? null
            };

            _context.Komentars.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = komentarViewModel.VijestId });
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(int vijestId, int stars)
        {
            var user = await _userManager.GetUserAsync(User);
            var rating = new Ocjena
            {
                Zvijezda = stars,
                KorisnikId = user.Id,
                VijestId = vijestId
            };

            var newss = await _context.Vijests.Include(n => n.Ocjenas).AsNoTracking().FirstOrDefaultAsync(n => n.VijestId == rating.VijestId);
            bool ocjenjen = false;
            var updatedRating = new Ocjena();
            foreach (var item in newss.Ocjenas)
            {
                if (item.KorisnikId == rating.KorisnikId)
                {
                    updatedRating.OcjenaId = item.OcjenaId;
                    ocjenjen = true; break;
                }
            }

            if (ocjenjen)
            {
                updatedRating.Zvijezda = stars;
                updatedRating.KorisnikId = user.Id;
                updatedRating.VijestId = vijestId;
                //_context.Ocjenas.Update(updatedRating);
                _ocjenaRepository.Update(updatedRating);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Ocjenas.Add(rating);
                await _context.SaveChangesAsync();
            }

            var news = await _context.Vijests.Include(n => n.Ocjenas).FirstOrDefaultAsync(n => n.VijestId == vijestId);
            news.ProsjecnaOcjena = news.Ocjenas.Average(r => r.Zvijezda);

            _context.Update(news);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = vijestId });
        }

        public async Task<IActionResult> ObrisiKomentar(int id, int vijestId)
        {
            var komentari = await _komentarRepository.GetAll();
            foreach (var item in komentari.Where(k => k.NadKomentar == id || k.KomentarId == id))
            {
                _komentarRepository.Delete(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = vijestId });
        }

        //public async Task<IActionResult> EditKomentar(int id, int vijestId)
        //{
        //    var komentar = await _komentarRepository.GetByIdAsync(id);
        //    _komentarRepository.Update(komentar);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Details), new { id = vijestId });
        //}

        private async Task< List<KomentarVijestiViewModel>> GetReplies(int commentId, List<Komentar> allComments, int vijestId)
        {
            var podKomentari = allComments
                .Where(c => c.NadKomentar == commentId)
                .OrderBy(c => c.Vrijeme)
                .Select(c => new KomentarVijestiViewModel
                {
                    KomentarId = c.KomentarId,
                    Sadrzaj = c.Sadrzaj,
                    Vrijeme = c.Vrijeme,
                    VijestId = c.VijestId,
                    KorisnikId = c.KorisnikId,
                    Replies = GetReplies(c.KomentarId, allComments, vijestId).Result
                })
                .ToList();

            var news = await _context.Vijests
                .Include(n => n.Komentars)
                .Include(n => n.Ocjenas)
                .FirstOrDefaultAsync(n => n.VijestId == vijestId);

            var i = 0;
            foreach (var item in news.Komentars.Where(c => c.NadKomentar == commentId))
            {
                var korisnikk = await _context.AppUsers.FirstOrDefaultAsync(b => b.Id == item.KorisnikId);
                podKomentari[i].ImeKorisnika = korisnikk.Ime;
                podKomentari[i].PrezimeKorisnika = korisnikk.Prezime;
                i++;
            }
            return podKomentari;
        }

        public async Task<IActionResult> GetEditPartial(int id)
        {
            var comment = await _context.Komentars.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return PartialView("EditCommentPartial", comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKomentar(int id, [Bind("KomentarId,Sadrzaj")] Komentar comment)
        {
            if (id != comment.KomentarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.KomentarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return PartialView("CommentPartial", comment);
            }
            return PartialView("EditCommentPartial", comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Komentars.Any(e => e.KomentarId == id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Vijest vijest = await _vijestRepository.GetByIdAsyncNoTracking(id);
            if (vijest == null) return View("Error");

            return View(vijest);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteVijest(int id)
        {
            Vijest vijest = await _vijestRepository.GetByIdAsync(id);

            var ocjene = await _ocjenaRepository.GetAll();
            var komentari = await _komentarRepository.GetAll();

            foreach (var item in ocjene)
            {
                if (item.VijestId == vijest.VijestId)
                {
                   _ocjenaRepository.Delete(item);
                }
            }
            foreach (var item in komentari)
            {
                if (item.VijestId == vijest.VijestId)
                {
                    _komentarRepository.Delete(item);
                }
            }

            _vijestRepository.Delete(vijest);
            return RedirectToAction("Index");
        }
    }
}
