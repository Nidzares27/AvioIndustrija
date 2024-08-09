using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IKomentarRepository
    {
        Task<IEnumerable<Komentar>> GetAll();
        Task<Komentar> GetByIdAsync(int id);
        Task<Komentar> GetByIdAsyncNoTracking(int id);
        bool Delete(Komentar komentar);
        bool Add(Komentar komentar);
        bool Update(Komentar komentar);
        bool Save();
    }
}
