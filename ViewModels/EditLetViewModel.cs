using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class EditLetViewModel
    {
        [Display(Name = "Let ID")]
        public int LetId { get; set; }
        [Display(Name = "Broj Leta")]
        public string BrojLeta { get; set; } = null!;
        [Display(Name = "Avion ID")]
        public int AvionId { get; set; }
        [Display(Name = "Relacija ID")]
        public int RelacijaId { get; set; }
        [Display(Name = "Vrijeme Poletanja")]
        public DateTime VrijemePoletanja { get; set; }
        [Display(Name = "Vrijeme Sletanja")]
        public DateTime VrijemeSletanja { get; set; }
        //public virtual Avion Avion { get; set; } = null!;
        //public virtual Relacija Relacija { get; set; } = null!;
    }
}
