using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class VijestRepository : IVijestRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public VijestRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Vijest vijest)
        {
            _context.Add(vijest);
            return Save();
        }

        public bool Delete(Vijest vijest)
        {
            _context?.Remove(vijest);
            return Save();
        }

        public async Task<IEnumerable<Vijest>> GetAll()
        {
            return await _context.Vijests.ToListAsync();
        }

        public async Task<Vijest> GetByIdAsync(int id)
        {
            return await _context.Vijests.FirstOrDefaultAsync(i => i.VijestId == id);
        }

        public async Task<Vijest> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Vijests.AsNoTracking().FirstOrDefaultAsync(i => i.VijestId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Vijest vijest)
        {
            _context.Update(vijest);
            return Save();
        }
    }
}
