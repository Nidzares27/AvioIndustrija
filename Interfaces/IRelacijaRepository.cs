using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IRelacijaRepository
    {
        Task<IEnumerable<Relacija>> GetAll();
        Task<IEnumerable<Relacija>> GetAllNoTracking();
        Task<Relacija> GetByIdAsync(int id);
        Task<Relacija> GetByIdAsyncNoTracking(int id);
        bool Delete(Relacija relacija);
        bool Add(Relacija relacija);
        bool Update(Relacija relacija);
        bool Save();
    }
}
