using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AvioIndustrija.Repository
{
    public class PutnikRepository : IPutnikRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public PutnikRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Putnik putnik)
        {
            _context.Add(putnik);
            return Save();
        }

        public bool Delete(Putnik putnik)
        {
            _context?.Remove(putnik);
            return Save();
        }

        public async Task<IEnumerable<Putnik>> GetAll()
        {
            return await _context.Putniks.ToListAsync();
        }

        public async Task<Putnik> GetByIdAsync(int id)
        {
            return await _context.Putniks.FirstOrDefaultAsync(i => i.PutnikId == id);
        }

        public async Task<Putnik> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Putniks.AsNoTracking().FirstOrDefaultAsync(i => i.PutnikId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Putnik putnik)
        {
            _context.Update(putnik);
            return Save();
        }
    }
}
