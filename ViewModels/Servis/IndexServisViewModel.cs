using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AvioIndustrija.Models;
using AvioIndustrija.ViewModels.Djelovi;

namespace AvioIndustrija.ViewModels.Servis
{
    public class IndexServisViewModel
    {
        public int ServisId { get; set; }
        [Column("AvionID")]
        public int AvionId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DatumServisa { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Status { get; set; } = null!;
        public string? Komentar { get; set; }
        public List<DioServisaSaKolicinomViewModel> ServisDjelovi { get; set; }
    }
}
