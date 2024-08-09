using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class DetailsIstorijaLetovaPutnikViewModel
    {
        [Display(Name = "Ime Putnika")]
        public string ImePutnika { get; set; }
        [Display(Name = "Broj Leta")]
        public string BrojLeta { get; set; }
        [Display(Name = "Redni Broj Sjedišta")]
        public short RedniBrojSjedišta { get; set; }
        public string Klasa { get; set; } = null!;
        [Display(Name = "Ručni Prtljag (<8kg)")]
        public string? RučniPrtljag8kg { get; set; }
        [Display(Name = "Koferi (kg)")]
        [Range(0, 30)]
        public byte? KoferiKg { get; set; }
    }
}
