using AvioIndustrija.Models;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AvioIndustrija.Interfaces
{
    public interface IDioRepository
    {
        Task<IEnumerable<Dio>> GetAll();
        Task<Dio> GetByIdAsync(int id);
        Task<Dio> GetByIdAsyncNoTracking(int id);
        bool Delete(Dio dio);
        bool Add(Dio dio);
        bool Update(Dio dio);
        bool Save();
    }
}
