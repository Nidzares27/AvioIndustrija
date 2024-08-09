using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class DioRepository : IDioRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public DioRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Dio dio)
        {
            _context.Add(dio);
            return Save();
        }

        public bool Delete(Dio dio)
        {
            _context?.Remove(dio);
            return Save();
        }

        public async Task<IEnumerable<Dio>> GetAll()
        {
            return await _context.Dios.ToListAsync();
        }

        public async Task<Dio> GetByIdAsync(int id)
        {
            return await _context.Dios.FirstOrDefaultAsync(i => i.SerijskiBroj == id);
        }

        public async Task<Dio> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Dios.AsNoTracking().FirstOrDefaultAsync(i => i.SerijskiBroj == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Dio dio)
        {
            _context.Update(dio);
            return Save();
        }
    }
}

