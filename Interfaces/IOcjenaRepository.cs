using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IOcjenaRepository
    {
        Task<IEnumerable<Ocjena>> GetAll();
        Task<Ocjena> GetByIdAsync(int id);
        Task<Ocjena> GetByIdAsyncNoTracking(int id);
        bool Delete(Ocjena ocjena);
        bool Add(Ocjena ocjena);
        bool Update(Ocjena ocjena);
        bool Save();
    }
}
