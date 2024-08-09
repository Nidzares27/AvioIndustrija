using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AvioIndustrija.Repository
{
    public class AvionRepository : IAvioniRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public AvionRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<Avion>> GetAll()
        {
            return await _context.Avions.ToListAsync();
        }

        public async Task<Avion> GetByIdAsync(int id)
        {
            return await _context.Avions.FirstOrDefaultAsync(i => i.AvionId == id);
        }

        public async Task<Avion> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Avions.AsNoTracking().FirstOrDefaultAsync(i => i.AvionId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Delete(Avion avion)
        {
            _context?.Remove(avion);
            return Save();
        }

        public bool Add(Avion avion)
        {
            _context.Add(avion);
            return Save();
        }

        public bool Update(Avion avion)
        {
            _context.Update(avion);
            return Save();
        }

    }
}
