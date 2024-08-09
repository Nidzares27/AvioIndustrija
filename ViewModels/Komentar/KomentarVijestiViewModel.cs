using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels.Komentar
{
    public class KomentarVijestiViewModel
    {
        public int KomentarId { get; set; }
        public string Sadrzaj { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime Vrijeme { get; set; }
        [Column("VijestID")]
        public int VijestId { get; set; }
        [Column("KorisnikID")]
        [StringLength(450)]
        public string KorisnikId { get; set; } = null!;
        public string ImeKorisnika { get; set; }
        public string PrezimeKorisnika { get; set; }
        public List<KomentarVijestiViewModel> Replies { get; set; }
    }
}
