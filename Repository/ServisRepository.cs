using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class ServisRepository : IServisRepository
    {
        private readonly AvioIndustrija2424Context _context;
        public ServisRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Servi servi)
        {
            _context.Add(servi);
            return Save();
        }

        public bool Delete(Servi servi)
        {
            _context?.Remove(servi);
            return Save();
        }

        public async Task<IEnumerable<Servi>> GetAll()
        {
            return await _context.Servis.ToListAsync();
        }

        public async Task<Servi> GetByIdAsync(int id)
        {
            return await _context.Servis.FirstOrDefaultAsync(i => i.ServisId == id);
        }

        public async Task<Servi> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Servis.AsNoTracking().FirstOrDefaultAsync(i => i.ServisId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Servi servi)
        {
            _context.Update(servi);
            return Save();
        }
    }
}

