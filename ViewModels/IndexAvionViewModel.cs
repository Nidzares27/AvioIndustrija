using AvioIndustrija.Models;
using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class IndexAvionViewModel
    {
        public PaginatedList<Avion> PaginatedItems { get; set; }
        public IEnumerable<Avion> AllItems { get; set; } = new List<Avion>();
        public int ActiveClassCounter { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchString { get; set; }
        public string? CurrentFilter { get; set; }
        public int? PageNumber { get; set; }

    }
}
