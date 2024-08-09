using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class CreateIstorijaLetovaPutnikaViewModel
    {
        [Display(Name = "Putnik ID")]
        public int PutnikId { get; set; }
        [Display(Name = "Let ID")]
        public int LetId { get; set; }
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
