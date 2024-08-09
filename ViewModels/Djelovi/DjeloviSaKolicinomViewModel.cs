using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels.Djelovi
{
    public class DioSaKolicinomViewModel
    {
        public int SerijskiBroj { get; set; }
        [StringLength(50)]
        public string Naziv { get; set; } = null!;
        [StringLength(50)]
        public string Model { get; set; } = null!;
        [StringLength(50)]
        public string Proizvodjac { get; set; } = null!;
        public int GodinaProizvodnja { get; set; }
        public int? Kolicina { get; set; }
    }
}

