using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface ILetRepository
    {
        Task<IEnumerable<Let>> GetAll();
        Task<Let> GetByIdAsync(int id);
        Task<Let> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Let>> GetByAvionAsync(int avion);
        Task<IEnumerable<Let>> GetByDateRange(DateTime OD, DateTime DO);
        bool Delete(Let let);
        bool Add(Let let);
        bool Update(Let let);
        bool Save();
    }
}
