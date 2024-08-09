using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class IstorijaLetovaPutnikaRepository : IIstorijaLetovaPutnikaRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public IstorijaLetovaPutnikaRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }

        public bool Add(IstorijaLetovaPutnika istorijaLetovaPutnika)
        {
            _context.Add(istorijaLetovaPutnika);
            return Save();
        }

        public bool Delete(IstorijaLetovaPutnika istorijaLetovaPutnika)
        {
            _context?.Remove(istorijaLetovaPutnika);
            return Save();
        }

        public async Task<IEnumerable<IstorijaLetovaPutnika>> GetAll()
        {
            return await _context.IstorijaLetovaPutnikas.ToListAsync();
        }

        public async Task<IstorijaLetovaPutnika> GetByIdAsync(int putnikID, int letID)
        {
            return await _context.IstorijaLetovaPutnikas.FirstOrDefaultAsync(i => i.PutnikId == putnikID && i.LetId == letID); 
        }

        public async Task<IstorijaLetovaPutnika> GetByIdAsyncNoTracking(int putnikID, int letID)
        {
            return await _context.IstorijaLetovaPutnikas.AsNoTracking().FirstOrDefaultAsync(i => i.PutnikId == putnikID && i.LetId == letID);
        }

        public async Task<IEnumerable<IstorijaLetovaPutnika>> GetByLetAsync(int let)
        {
            return await _context.IstorijaLetovaPutnikas.AsNoTracking().Where(c => c.LetId.Equals(let)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(IstorijaLetovaPutnika istorijaLetovaPutnika)
        {
            _context.Update(istorijaLetovaPutnika);
            return Save();
        }
    }
}
