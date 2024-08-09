using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AvioIndustrija.ViewModels
{
    public class DetailsLetViewModel
    {
        [Display(Name = "Let ID")]
        public int LetId { get; set; }
        [Display(Name = "Broj Leta")]
        public string BrojLeta { get; set; } = null!;
        [Display(Name = "Ime Aviona")]
        public string ImeAviona { get; set; }
        [Display(Name = "Relacija")]
        public string Relacija { get; set; }
        [Display(Name = "Vrijeme Poletanja")]
        public DateTime VrijemePoletanja { get; set; }
        [Display(Name = "Vrijeme Sletanja")]
        public DateTime VrijemeSletanja { get; set; }
    }
}
