using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AvioIndustrija.ViewModels
{
    public class CreateAvionViewModel
    {
        public int AvionId { get; set; }
        [Display(Name = "Ime Aviona")]
        public string ImeAviona { get; set; } = null!;
        public string Kompanija { get; set; } = null!;
        [Display(Name = "Godina Proizvodnje")]
        public short GodinaProizvodnje { get; set; }
        [Display(Name = "Visina (m)")]
        [Range(20, 60)]
        public short VisinaM { get; set; }
        [Display(Name = "Širina (m)")]
        [Range(20, 150)]
        public short ŠirinaM { get; set; }
        [Display(Name = "Dužina (m)")]
        [Range(20, 150)]
        public short DužinaM { get; set; }
        [Display(Name = "Broj Sjedišta Biznis Klase")]
        [Range(0, 200)]
        public short BrojSjedištaBiznisKlase { get; set; }
        [Display(Name = "Broj Sjedišta Ekonomske Klase")]
        [Range(0, 300)]
        public short BrojSjedištaEkonomskeKlase { get; set; }
        [Display(Name = "Nosivost (kg)")]
        [Range(1000, 10000)]
        public short NosivostKg { get; set; }
        [Display(Name = "KapacitetRezervoara (L)")]
        [Range(500, 5000)]
        public short KapacitetRezervoaraL { get; set; }
        [Display(Name = "Maksimalni Domet (km)")]
        [Range(1000, 5000)]
        public short MaksimalniDometKm { get; set; }
        [Display(Name = "Image")]
        public IFormFile? ImageUrl { get; set; }
    }

}
