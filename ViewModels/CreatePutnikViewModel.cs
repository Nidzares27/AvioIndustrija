using System.ComponentModel.DataAnnotations;

namespace AvioIndustrija.ViewModels
{
    public class CreatePutnikViewModel
    {
        public int PutnikId { get; set; }
        public string Ime { get; set; } = null!;
        public string Prezime { get; set; } = null!;
        public string? Pol { get; set; }
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Molimo unesite vaildan email!")]
        public string? Email { get; set; }
        public IFormFile? Image { get; set; }
        public string? CropData { get; set; }
    }
}
