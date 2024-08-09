using AvioIndustrija.Models;
using System.Diagnostics;

namespace AvioIndustrija.Interfaces
{
    public interface IAvioniRepository
    {
        Task<IEnumerable<Avion>> GetAll();
        Task<Avion> GetByIdAsync(int id);
        Task<Avion> GetByIdAsyncNoTracking(int id);
        bool Delete(Avion avion);
        bool Add(Avion avion);
        bool Update(Avion avion);
        bool Save();
    }
}
