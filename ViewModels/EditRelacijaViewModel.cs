using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class EditRelacijaViewModel
    {
        [Display(Name = "Relacija ID")]
        public int RelacijaId { get; set; }
        [Display(Name = "Grad (od)")]
        public string GradOd { get; set; } = null!;
        [Display(Name = "Država (od)")]
        public string DržavaOd { get; set; } = null!;
        [Display(Name = "Aerodrom (od)")]
        public string AerodromOd { get; set; } = null!;
        [Display(Name = "Grad (do)")]
        public string GradDo { get; set; } = null!;
        [Display(Name = "Država (do)")]
        public string DržavaDo { get; set; } = null!;
        [Display(Name = "Aerodrom (do)")]
        public string AerodromDo { get; set; } = null!;
        [Display(Name = "Udaljenost (km)")]
        [Range(100, 5000)]
        public short UdaljenostKm { get; set; }
    }
}
