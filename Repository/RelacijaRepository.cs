using AvioIndustrija.Data;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AvioIndustrija.Repository
{
    public class RelacijaRepository : IRelacijaRepository
    {
        private readonly AvioIndustrija2424Context _context;

        public RelacijaRepository(AvioIndustrija2424Context context)
        {
            this._context = context;
        }
        public bool Add(Relacija relacija)
        {
            _context.Add(relacija);
            return Save();
        }

        public bool Delete(Relacija relacija)
        {
            _context?.Remove(relacija);
            return Save();
        }

        public async Task<IEnumerable<Relacija>> GetAll()
        {
            return await _context.Relacijas.ToListAsync();
        }
        public async Task<IEnumerable<Relacija>> GetAllNoTracking()
        {
            return await _context.Relacijas.AsNoTracking().ToListAsync();
        }

        public async Task<Relacija> GetByIdAsync(int id)
        {
            return await _context.Relacijas.FirstOrDefaultAsync(i => i.RelacijaId == id);
        }

        public async Task<Relacija> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Relacijas.AsNoTracking().FirstOrDefaultAsync(i => i.RelacijaId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Relacija relacija)
        {
            _context.Update(relacija);
            return Save();
        }
    }
}
