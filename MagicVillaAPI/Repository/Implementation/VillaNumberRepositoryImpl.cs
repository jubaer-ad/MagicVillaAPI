using MagicVillaAPI.DBContext;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaAPI.Repository.Implementation
{
    public class VillaNumberRepositoryImpl : RepositoryImpl<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaNumberRepositoryImpl(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
} 
