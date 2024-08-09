using AvioIndustrija.Models;

namespace AvioIndustrija.ViewModels
{
    public class Validni_NevalidniAvioni
    {
        public List<NevalidniAvionViewModel> nevalidni { get; set; }
        public List<Avion> validni { get; set; }
        public List<Avion>? avioniZaUnos { get; set; }


    }
}
