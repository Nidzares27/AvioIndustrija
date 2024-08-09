using AvioIndustrija.Models;

namespace AvioIndustrija.ViewModels
{
    public class IzvjestajOLetovimaViewModel
    {
        public string DatumOd { get; set; }
        public string DatumDo { get; set; }
        public IEnumerable<Avion> najcesciAvioni { get; set; }
        public IEnumerable<Relacija> najcesceRelacije { get; set; }
        public int brojLetova { get; set; }
        public int brojLetovaNajcescihAviona { get; set; }
        public int brojLetovaNajcescimRelacijama { get; set; }
        public IEnumerable<Let> letovi { get; set; }
    }
}

