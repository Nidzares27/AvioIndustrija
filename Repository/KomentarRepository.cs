using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class KomentarRepository : IKomentarRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public KomentarRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Komentar komentar)
        {
            _context.Add(komentar);
            return Save();
        }

        public bool Delete(Komentar komentar)
        {
            _context?.Remove(komentar);
            return Save();
        }

        public async Task<IEnumerable<Komentar>> GetAll()
        {
            return await _context.Komentars.ToListAsync();
        }

        public async Task<Komentar> GetByIdAsync(int id)
        {
            return await _context.Komentars.FirstOrDefaultAsync(i => i.KomentarId == id);
        }

        public async Task<Komentar> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Komentars.AsNoTracking().FirstOrDefaultAsync(i => i.KomentarId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Komentar komentar)
        {
            _context.Update(komentar);
            return Save();
        }
    }
}
