using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IPutnikRepository
    {
        Task<IEnumerable<Putnik>> GetAll();
        Task<Putnik> GetByIdAsync(int id);
        Task<Putnik> GetByIdAsyncNoTracking(int id);
        bool Delete(Putnik putnik);
        bool Add(Putnik putnik);
        bool Update(Putnik putnik);
        bool Save();
    }
}
