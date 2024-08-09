using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IServisDioRepository
    {
        Task<IEnumerable<ServisDio>> GetAll();
        Task<IEnumerable<ServisDio>> GetAllWhere(int servisId);
        Task<IEnumerable<ServisDio>> GetAllWhereNoTracking(int servisId);
        Task<ServisDio> GetByIdAsync(int servisId, int serijskiBroj);
        Task<ServisDio> GetByIdAsyncNoTracking(int servisId, int serijskiBroj);
        bool Delete(ServisDio servisDio);
        bool Add(ServisDio servisDio);
        bool Update(ServisDio servisDio);
        bool Save();
    }
}
