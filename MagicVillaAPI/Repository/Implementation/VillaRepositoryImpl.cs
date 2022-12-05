using MagicVillaAPI.DBContext;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaAPI.Repository.Implementation
{
    public class VillaRepositoryImpl : RepositoryImpl<Villa>, IVillaRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaRepositoryImpl(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
