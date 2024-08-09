using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Repository
{
    public class ServisDioRepository : IServisDioRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public ServisDioRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(ServisDio servisDio)
        {
            _context.Add(servisDio);
            return Save();
        }

        public bool Delete(ServisDio servisDio)
        {
            _context?.Remove(servisDio);
            return Save();
        }

        public async Task<IEnumerable<ServisDio>> GetAll()
        {
            return await _context.ServisDios.ToListAsync();
        }

        public async Task<IEnumerable<ServisDio>> GetAllWhere(int servisId)
        {
            return await _context.ServisDios.Where(i => i.ServisId == servisId).ToListAsync();
        }

        public async Task<IEnumerable<ServisDio>> GetAllWhereNoTracking(int servisId)
        {
            return await _context.ServisDios.AsNoTracking().Where(i => i.ServisId == servisId).ToListAsync();
        }

        public async Task<ServisDio> GetByIdAsync(int servisId, int serijskiBroj)
        {
            return await _context.ServisDios.FirstOrDefaultAsync(i => i.ServisId == servisId && i.SerijskiBroj == serijskiBroj);
        }

        public async Task<ServisDio> GetByIdAsyncNoTracking(int servisId, int serijskiBroj)
        {
            return await _context.ServisDios.AsNoTracking().FirstOrDefaultAsync(i => i.ServisId == servisId && i.SerijskiBroj == serijskiBroj);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ServisDio servisDio)
        {
            _context.Update(servisDio);
            return Save();
        }
    }
}

