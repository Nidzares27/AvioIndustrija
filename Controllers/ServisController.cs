using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.ViewModels;
using AvioIndustrija.ViewModels.Djelovi;
using AvioIndustrija.ViewModels.Servis;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AvioIndustrija.Controllers
{
    public class ServisController : Controller
    {
        private readonly AvioIndustrija2424Context _context;
        private readonly IServisRepository _servisRepository;
        private readonly IDioRepository _dioRepository;
        private readonly IServisDioRepository _servisDioRepository;

        public ServisController(AvioIndustrija2424Context context, IServisRepository servisRepository, IDioRepository dioRepository, IServisDioRepository servisDioRepository)
        {
            this._context = context;
            this._servisRepository = servisRepository;
            this._dioRepository = dioRepository;
            this._servisDioRepository = servisDioRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Servi> servisi = await _servisRepository.GetAll();
            var servisii = new List<IndexServisViewModel>();
            foreach (var s in servisi)
            {
                var servis = new IndexServisViewModel
                {
                    ServisId = s.ServisId,
                    AvionId = s.AvionId,
                    DatumServisa = s.DatumServisa,
                    Status = s.Status,
                    Komentar = s.Komentar,
                };

                IEnumerable<ServisDio> servisDjelovi = await _servisDioRepository.GetAllWhere(s.ServisId);
                var djeloviServisa = new List<DioServisaSaKolicinomViewModel>();
                foreach (var item in servisDjelovi)
                {
                    var d = await _dioRepository.GetByIdAsync(item.SerijskiBroj);
                    var ds = new DioServisaSaKolicinomViewModel
                    {
                        Naziv = d.Naziv,
                        Kolicina = item.Kolicina
                    };
                    djeloviServisa.Add(ds);
                }
                servis.ServisDjelovi = djeloviServisa;

                servisii.Add(servis);
            }
            return View(servisii);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateServisViewModel servisViewModel)
        {
            if (ModelState.IsValid)
            {
                DateTime datumServisa = DateTime.Parse(servisViewModel.DatumServisa);

                var servis = new Servi
                {
                    AvionId = servisViewModel.AvionId,
                    DatumServisa = datumServisa,
                    Status = "Neizvrsen",
                    Komentar = ""
                };

                _servisRepository.Add(servis);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Kreiranje servisa nije uspjelo");
            }
            return RedirectToAction("Index", "Avioni");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {

            Servi servis = await _servisRepository.GetByIdAsyncNoTracking(id);
            if (servis == null) return View("Error");

            IEnumerable<ServisDio> servisDjelovi = await _servisDioRepository.GetAllWhere(id);
            var djeloviServisaId = new List<int>();
            var djeloviServisa = new List<DioSaKolicinomViewModel>();
            foreach (var item in servisDjelovi)
            {
                var d = await _dioRepository.GetByIdAsync(item.SerijskiBroj);
                djeloviServisaId.Add(d.SerijskiBroj);
                var dioSaKolicinom = new DioSaKolicinomViewModel
                {
                    SerijskiBroj = d.SerijskiBroj,
                    Naziv = d.Naziv,
                    Model = d.Model,
                    Proizvodjac = d.Proizvodjac,
                    GodinaProizvodnja = d.GodinaProizvodnja,
                    Kolicina = item.Kolicina,
                };
                djeloviServisa.Add(dioSaKolicinom);
            }

            var servisViewModel = new EditServisViewModel
            {
                ServisId = servis.ServisId,
                AvionId = servis.AvionId,
                DatumServisa = servis.DatumServisa,
                Status = servis.Status,
                Komentar = servis.Komentar,
                SviDjelovi = await _context.Dios.ToListAsync(),
                IzabraniDjelovi = djeloviServisa,
                IzabraniDjeloviId = djeloviServisaId ?? null

            };
            return View(servisViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditServisViewModel servisViewModel)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Editovanje servisa nije uspjelo");
                return View("Edit", servisViewModel);
            }

            var servis = await _servisRepository.GetByIdAsyncNoTracking(id);

            if (servis != null)
            {
                var servisEdit = new Servi
                {
                    ServisId = id,
                    AvionId = servisViewModel.AvionId,
                    DatumServisa = servisViewModel.DatumServisa,
                    Status = servisViewModel.Status,
                    Komentar = servisViewModel.Komentar,
                };

                _servisRepository.Update(servisEdit);

                IEnumerable<ServisDio> servisDjelovi = await _servisDioRepository.GetAllWhereNoTracking(id);

                //Dodavanje Djelova
                int brojac = 0;
                foreach (var item in servisViewModel.IzabraniDjeloviId)
                {
                    var dodati = true;
                    foreach (var sd in servisDjelovi)
                    {
                        if (item == sd.SerijskiBroj)
                        {
                            if (servisViewModel.SelectedPartQuantities[brojac] != sd.Kolicina)//Update Kolicine item.kolicina
                            {
                                var servisDioUpdate = new ServisDio
                                {
                                    ServisId = servisViewModel.ServisId,
                                    SerijskiBroj = item,
                                    Kolicina = servisViewModel.SelectedPartQuantities[brojac],
                                };

                                _servisDioRepository.Update(servisDioUpdate);
                            }
                            dodati = false;
                            break;
                        }
                    }
                    if (dodati)
                    {
                        var servisDio = new ServisDio
                        {
                            ServisId = servisViewModel.ServisId,
                            SerijskiBroj = item,
                            Kolicina = servisViewModel.SelectedPartQuantities[brojac],
                        };

                        _servisDioRepository.Add(servisDio);
                    }
                    brojac++;
                }

                //Brisanje Djelova
                foreach (var item in servisDjelovi)
                {
                    int brojacc = 0;
                    var izbrisati = true;
                    foreach (var y in servisViewModel.IzabraniDjeloviId)
                    {
                        if (y == item.SerijskiBroj)
                        {
                            brojac++;
                            izbrisati = false;
                            break;
                        }
                    }
                    if (izbrisati)
                    {
                        var servisDioIzbrisati = new ServisDio
                        {
                            ServisId = item.ServisId,
                            SerijskiBroj = item.SerijskiBroj,
                            Kolicina = servisViewModel.SelectedPartQuantities[brojacc],
                        };

                        _servisDioRepository.Delete(servisDioIzbrisati);
                        break;
                    }
                }

                return RedirectToAction("Index");

            }
            else
            {
                return View(servisViewModel);
            }
        }
    }
}
