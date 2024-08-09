using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class OcjenaRepository : IOcjenaRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public OcjenaRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Ocjena ocjena)
        {
            _context.Add(ocjena);
            return Save();
        }

        public bool Delete(Ocjena ocjena)
        {
            _context?.Remove(ocjena);
            return Save();
        }

        public async Task<IEnumerable<Ocjena>> GetAll()
        {
            return await _context.Ocjenas.ToListAsync();
        }

        public async Task<Ocjena> GetByIdAsync(int id)
        {
            return await _context.Ocjenas.FirstOrDefaultAsync(i => i.OcjenaId == id);
        }

        public async Task<Ocjena> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Ocjenas.AsNoTracking().FirstOrDefaultAsync(i => i.OcjenaId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Ocjena ocjena)
        {
            _context.Update(ocjena);
            return Save();
        }
    }
}
