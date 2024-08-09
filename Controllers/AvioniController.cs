using AvioIndustrija.Data;
using AvioIndustrija.Helpers;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.Services;
using AvioIndustrija.Utils;
using AvioIndustrija.ViewModels;
using ClosedXML.Excel;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace AvioIndustrija.Controllers
{

    [Authorize]
    public class AvioniController : Controller
    {
        private readonly AvioIndustrija2424Context _context;
        private readonly IAvioniRepository _avioniRepository;
        private readonly IPhotoService _photoService;
        private readonly ExcelService _excelService;
        private readonly AvionValidation _avionValidation;
        private readonly ListaNevalidnihAviona _listaNevalidnihAviona;
        private readonly HttpClient _httpClient;

        public AvioniController(AvioIndustrija2424Context context, IAvioniRepository avioniRepository, IPhotoService photoService, ExcelService excelService, AvionValidation avionValidation, ListaNevalidnihAviona listaNevalidnihAviona, HttpClient httpClient)
        {
            this._context = context;
            this._avioniRepository = avioniRepository;
            this._photoService = photoService;
            this._excelService = excelService;
            this._avionValidation = avionValidation;
            this._listaNevalidnihAviona = listaNevalidnihAviona;
            this._httpClient = httpClient;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            IEnumerable<Avion> avs = await _avioniRepository.GetAll();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ImeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ime_desc" : "";
            ViewData["GodinaProizvodnjeSortParm"] = sortOrder == "Godina" ? "godina_desc" : "Godina";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var avioni = from a in _context.Avions
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                avioni = avioni.Where(a => a.ImeAviona.Contains(searchString)
                                       || a.Kompanija.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "ime_desc":
                    avioni = avioni.OrderByDescending(a => a.ImeAviona);
                    break;
                case "Godina":
                    avioni = avioni.OrderBy(a => a.GodinaProizvodnje);
                    break;
                case "godina_desc":
                    avioni = avioni.OrderByDescending(a => a.GodinaProizvodnje);
                    break;
                default:
                    avioni = avioni.OrderBy(a => a.ImeAviona);
                    break;
            }

            HttpContext.Session.SetObject("MyList", avioni);

            int pageSize = 8;
            var PaginatedItems = await PaginatedList<Avion>.CreateAsync(avioni, pageNumber ?? 1, pageSize);
            var VM = new IndexAvionViewModel
            {
                PaginatedItems = PaginatedItems,
                AllItems = avs,
                ActiveClassCounter = 0,
                SortOrder = sortOrder,
                SearchString = searchString,
                CurrentFilter = currentFilter,
                PageNumber = pageNumber,

            };
            return View(VM);
        }
        public async Task<IActionResult> Details(int id)
        {
            Avion avion = await _avioniRepository.GetByIdAsyncNoTracking(id);
            return View(avion);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Avion avion = await _avioniRepository.GetByIdAsyncNoTracking(id);
            if (avion == null) return View("Error");
            var avionVM = new EditAvionViewModel
            {
                ImeAviona = avion.ImeAviona,
                Kompanija = avion.Kompanija,
                GodinaProizvodnje = avion.GodinaProizvodnje,
                VisinaM = avion.VisinaM,
                ŠirinaM = avion.ŠirinaM,
                DužinaM = avion.DužinaM,
                BrojSjedištaBiznisKlase = avion.BrojSjedištaBiznisKlase,
                BrojSjedištaEkonomskeKlase = avion.BrojSjedištaEkonomskeKlase,
                NosivostKg = avion.NosivostKg,
                KapacitetRezervoaraL = avion.KapacitetRezervoaraL,
                MaksimalniDometKm = avion.MaksimalniDometKm,
                URL = avion.ImageUrl,
            };
            return View(avionVM);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditAvionViewModel avionVM)
        {
            string currentYear = DateTime.Now.Year.ToString();
            int currYear = Int32.Parse(currentYear);
            var photoResult = new ImageUploadResult();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje aviona nije uspjelo");
                return View("Edit", avionVM);
            }

            var avion = await _avioniRepository.GetByIdAsyncNoTracking(id);

            if (avion != null)
            {
                if (avionVM.ImageUrl != null)
                {
                    if (!string.IsNullOrEmpty(avion.ImageUrl))
                    {
                        var publicId = CloudinaryHelper.GetPublicIdFromUrlFromFolder(avion.ImageUrl);

                        try
                        {
                            await _photoService.DeletePhotoAsync(publicId);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Could not delete photo");
                            return View(avionVM);
                        }
                    }
                }

                if (avionVM.ImageUrl != null)
                {
                    var fileExtension = Path.GetExtension(avionVM.ImageUrl.FileName).ToLowerInvariant();
                    var validImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };

                    if (!validImageTypes.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "Please upload a valid image file (JPEG, PNG, GIF).");
                        return View(avionVM);
                    }
                    photoResult = await _photoService.AddPhotoAsync(avionVM.ImageUrl, "Avioni");
                }

                var avionEdit = new Avion
                {
                    AvionId = id,
                    ImeAviona = avionVM.ImeAviona,
                    Kompanija = avionVM.Kompanija,
                    GodinaProizvodnje = avionVM.GodinaProizvodnje,
                    VisinaM = avionVM.VisinaM,
                    ŠirinaM = avionVM.ŠirinaM,
                    DužinaM = avionVM.DužinaM,
                    BrojSjedištaBiznisKlase = avionVM.BrojSjedištaBiznisKlase,
                    BrojSjedištaEkonomskeKlase = avionVM.BrojSjedištaEkonomskeKlase,
                    NosivostKg = avionVM.NosivostKg,
                    KapacitetRezervoaraL = avionVM.KapacitetRezervoaraL,
                    MaksimalniDometKm = avionVM.MaksimalniDometKm,
                    ImageUrl = avionVM.ImageUrl != null ? photoResult.Url.ToString() : avion.ImageUrl
                };

                if (avionVM.GodinaProizvodnje < 1950 || avionVM.GodinaProizvodnje > currYear)
                {
                    ViewData["error"] = "Godina proizvodnje aviona ne smije biti manja od 1950 ili veca od trenutne godine";
                    return View(avionVM);
                }

                _avioniRepository.Update(avionEdit);

                return RedirectToAction("Index");

            }
            else
            {
                return View(avionVM);
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Avion avion = await _avioniRepository.GetByIdAsyncNoTracking(id);
            return View(avion);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteAvion(int id)
        {
            Avion avion = await _avioniRepository.GetByIdAsync(id);

            if (!string.IsNullOrEmpty(avion.ImageUrl))
            {
                var publicId = CloudinaryHelper.GetPublicIdFromUrlFromFolder(avion.ImageUrl);

                try
                {
                    await _photoService.DeletePhotoAsync(publicId);
                }
                catch (Exception ex) // Da li dozvoliti brisanje?
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(id);
                }
            }

            _avioniRepository.Delete(avion);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAvionViewModel avionVM)
        {
            IEnumerable<Avion> avioni = await _avioniRepository.GetAll();
            string currentYear = DateTime.Now.Year.ToString();
            int currYear = Int32.Parse(currentYear);
            var result = new ImageUploadResult();

            if (ModelState.IsValid)
            {
                if (avionVM.ImageUrl != null)
                {
                    var fileExtension = Path.GetExtension(avionVM.ImageUrl.FileName).ToLowerInvariant();
                    var validImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };

                    if (!validImageTypes.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Image", "Please upload a valid image file (JPEG, PNG, GIF).");
                        return View(avionVM);
                    }
                    result = await _photoService.AddPhotoAsync(avionVM.ImageUrl, "Avioni");
                }

                var avion = new Avion
                {
                    ImeAviona = avionVM.ImeAviona,
                    Kompanija = avionVM.Kompanija,
                    GodinaProizvodnje = avionVM.GodinaProizvodnje,
                    VisinaM = avionVM.VisinaM,
                    ŠirinaM = avionVM.ŠirinaM,
                    DužinaM = avionVM.DužinaM,
                    BrojSjedištaBiznisKlase = avionVM.BrojSjedištaBiznisKlase,
                    BrojSjedištaEkonomskeKlase = avionVM.BrojSjedištaEkonomskeKlase,
                    NosivostKg = avionVM.NosivostKg,
                    KapacitetRezervoaraL = avionVM.KapacitetRezervoaraL,
                    MaksimalniDometKm = avionVM.MaksimalniDometKm,
                    ImageUrl = avionVM.ImageUrl != null ? result.Url.ToString() : null
                };

                if (avionVM.GodinaProizvodnje < 1950 || avionVM.GodinaProizvodnje > currYear)
                {
                    ViewData["error"] = "Godina proizvodnje aviona ne smije biti manja od 1950 ili veca od trenutne godine";
                    return View(avionVM);
                }

                foreach (var item in avioni)
                {
                    if (item.ImeAviona == avionVM.ImeAviona & item.Kompanija == avionVM.Kompanija)
                    {
                        ViewData["error"] = "Avion sa istim imenom i kompanijom vec postoji! Pokusaj ponovo.";
                        return View(avionVM);
                    }
                }

                _avioniRepository.Add(avion);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje aviona nije uspjelo");
            }
            return View(avionVM);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<FileResult> ExportAvionsExcel()
        {
            var avions = await _avioniRepository.GetAll();
            var fileName = "avions.xlsx";
            return GenerateExcel(fileName, avions);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<FileResult> ExportFilteredAvionsExcel()
        {
            var myList = HttpContext.Session.GetObject<List<Avion>>("MyList");
            var avs = (IEnumerable<Avion>)myList;
            var fileName = "FilteredAvions.xlsx";
            return GenerateExcel(fileName, avs);
        }
        [AllowAnonymous]
        public FileResult GenerateExcel(string fileName, IEnumerable<Avion> avions)
        {
            DataTable dataTable = new DataTable("Avioni");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("AvionID"),
                new DataColumn("Ime Aviona"),
                new DataColumn("Kompanija"),
                new DataColumn("Godina Proizvodnje"),
                new DataColumn("Visina (m)"),
                new DataColumn("Širina (m)"),
                new DataColumn("Dužina (m)"),
                new DataColumn("Broj Sjedišta Biznis Klase"),
                new DataColumn("Broj Sjedišta Ekonomske Klase"),
                new DataColumn("Nosivost (kg)"),
                new DataColumn("KapacitetRezervoara (L)"),
                new DataColumn("Maksimalni Domet (km)"),
                //new DataColumn("Image"),
            });

            foreach (var avion in avions)
            {
                dataTable.Rows.Add(avion.AvionId,
                    avion.ImeAviona,
                    avion.Kompanija,
                    avion.GodinaProizvodnje,
                    avion.VisinaM,
                    avion.ŠirinaM,
                    avion.DužinaM,
                    avion.BrojSjedištaBiznisKlase,
                    avion.BrojSjedištaEkonomskeKlase,
                    avion.NosivostKg,
                    avion.KapacitetRezervoaraL,
                    avion.MaksimalniDometKm
                    /*avion.ImageUrl*/);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, IndexAvionViewModel IAVM)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid Excel file.");
                return RedirectToAction("Index", new { sortOrder = IAVM.SortOrder, searchString = IAVM.SearchString, currentFilter = IAVM.CurrentFilter, pageNumber = IAVM.PageNumber });
            }

            // Check file extension and MIME type
            var supportedTypes = new[] { "xls", "xlsx" };
            var fileExtension = Path.GetExtension(file.FileName).Substring(1);
            var mimeType = file.ContentType;

            if (!supportedTypes.Contains(fileExtension) ||
                (mimeType != "application/vnd.ms-excel" && mimeType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                ModelState.AddModelError("File", "Please upload an Excel file with .xls or .xlsx extension.");
                return View();
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var models = _excelService.ImportFromExcelAsync(stream);

                IEnumerable<Avion> avioni = await _avioniRepository.GetAll();
                var azuriranaListaAviona = _avionValidation.ValidateAvions(models, avioni).Result;

                azuriranaListaAviona.avioniZaUnos = new List<Avion>();
                foreach (var item in azuriranaListaAviona.validni)
                {
                    azuriranaListaAviona.avioniZaUnos.Add(new Avion { });
                }

                return View("UploadSuccess", azuriranaListaAviona);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CompleteUpload(Validni_NevalidniAvioni validniAvioni)
        {
            foreach (var item in validniAvioni.avioniZaUnos)
            {
                _avioniRepository.Add(item);
            }
            TempData["SuccessMessage"] = "Avioni su uspjesno unijeti u bazu podataka!";
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CancelUpload(Validni_NevalidniAvioni validniAvioni)
        {
            foreach (var item in validniAvioni.avioniZaUnos)
            {
                if (!String.IsNullOrEmpty(item.ImageUrl))
                {
                    var publicId = CloudinaryHelper.GetPublicIdFromUrlFromFolder(item.ImageUrl);

                    try
                    {
                        await _photoService.DeletePhotoAsync(publicId);
                    }
                    catch (Exception ex) { }

                }
            }
            return RedirectToAction("Index");
        }

    }
}
