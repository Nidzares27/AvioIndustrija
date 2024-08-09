using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class LetRepository : ILetRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public LetRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Let let)
        {
            _context.Add(let);
            return Save();
        }

        public bool Delete(Let let)
        {
            _context?.Remove(let);
            return Save();
        }

        public async Task<IEnumerable<Let>> GetAll()
        {
            return await _context.Lets.ToListAsync();
        }

        public async Task<Let> GetByIdAsync(int id)
        {
            return await _context.Lets.FirstOrDefaultAsync(i => i.LetId == id);
        }

        public async Task<Let> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Lets.AsNoTracking().FirstOrDefaultAsync(i => i.LetId == id);
        }

        public async Task<IEnumerable<Let>> GetByAvionAsync(int avion)
        {
            return await _context.Lets.AsNoTracking().Where(c => c.AvionId.Equals(avion)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Let let)
        {
            _context.Update(let);
            return Save();
        }

        public async Task<IEnumerable<Let>> GetByDateRange(DateTime OD, DateTime DO)
        {
            return await _context.Lets.AsNoTracking().Where(c => c.VrijemePoletanja >= (OD) & c.VrijemeSletanja <= (DO)).ToListAsync();
        }

    }
}
