using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IServisRepository
    {
        Task<IEnumerable<Servi>> GetAll();
        Task<Servi> GetByIdAsync(int id);
        Task<Servi> GetByIdAsyncNoTracking(int id);
        bool Delete(Servi servi);
        bool Add(Servi servi);
        bool Update(Servi servi);
        bool Save();
    }
}

