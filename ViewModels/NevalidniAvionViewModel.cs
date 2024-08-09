using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class NevalidniAvionViewModel
    {
        public string? ImeAviona { get; set; }
        public string? Kompanija { get; set; }
        [Display(Name = "Godina Proizvodnje")]
        public string? GodinaProizvodnje { get; set; }
        [Display(Name = "Visina (m)")]
        public string? VisinaM { get; set; }
        [Display(Name = "Širina (m)")]
        public string? ŠirinaM { get; set; }
        [Display(Name = "Dužina (m)")]
        public string? DužinaM { get; set; }
        [Display(Name = "Broj Sjedišta Biznis Klase")]
        public string? BrojSjedištaBiznisKlase { get; set; }
        [Display(Name = "Broj Sjedišta Ekonomske Klase")]
        public string? BrojSjedištaEkonomskeKlase { get; set; }
        [Display(Name = "Nosivost (kg)")]
        public string? NosivostKg { get; set; }
        [Display(Name = "KapacitetRezervoara (L)")]
        public string? KapacitetRezervoaraL { get; set; }
        [Display(Name = "Maksimalni Domet (km)")]
        public string? MaksimalniDometKm { get; set; }
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
        public string? Greska { get; set; }


    }
}
