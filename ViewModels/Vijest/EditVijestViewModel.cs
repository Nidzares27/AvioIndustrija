using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels.Vijest
{
    public class EditVijestViewModel
    {
        public int VijestId { get; set; }
        public string Naslov { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime VrijemeObjave { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int BrojPregleda { get; set; }
        public double ProsjecnaOcjena { get; set; }
        [Column("KorisnikID")]
        [StringLength(450)]
        public string? KorisnikId { get; set; }
    }
}
