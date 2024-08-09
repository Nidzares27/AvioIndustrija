using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IVijestRepository
    {
        Task<IEnumerable<Vijest>> GetAll();
        Task<Vijest> GetByIdAsync(int id);
        Task<Vijest> GetByIdAsyncNoTracking(int id);
        bool Delete(Vijest vijest);
        bool Add(Vijest vijest);
        bool Update(Vijest vijest);
        bool Save();
    }
}
