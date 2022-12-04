using MagicVillaAPI.DBContext;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaAPI.Repository.Implementation
{
    public class VillaRepositoryImpl : IVillaRepository
    {
        private readonly VillaDBContext _db;
        public VillaRepositoryImpl(VillaDBContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Villa entity)
        {
            await _db.Villas.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;
            if (!tracked) query = query.AsNoTracking();
            if (filter != null) query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null)
        {
            IQueryable<Villa> query = _db.Villas;
            if (filter != null) query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Villa entity)
        {
            _db.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Villa> GetLastAsync()
        {
            return await _db.Villas.AsNoTracking().OrderBy(v => v.Id).LastAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            _db.Villas.Update(entity);
            await SaveAsync();
        }
    }
}
