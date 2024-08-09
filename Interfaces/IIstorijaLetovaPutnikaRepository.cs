using AvioIndustrija.Models;

namespace AvioIndustrija.Interfaces
{
    public interface IIstorijaLetovaPutnikaRepository
    {
        Task<IEnumerable<IstorijaLetovaPutnika>> GetAll();
        Task<IstorijaLetovaPutnika> GetByIdAsync(int putnikID, int letID);
        Task<IstorijaLetovaPutnika> GetByIdAsyncNoTracking(int putnikID, int letID);
        Task<IEnumerable<IstorijaLetovaPutnika>> GetByLetAsync(int let);
        bool Delete(IstorijaLetovaPutnika istorijaLetovaPutnika);
        bool Add(IstorijaLetovaPutnika istorijaLetovaPutnika);
        bool Update(IstorijaLetovaPutnika istorijaLetovaPutnika);
        bool Save();
    }
}
