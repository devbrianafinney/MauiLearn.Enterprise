using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Repository;
using MauiLearn.CoreEF.Data;
using Microsoft.EntityFrameworkCore;

namespace MauiLearn.CoreEF.Repository
{
    /*2/11/2026: determining from MauiLearn.Core.Services.Repository */
    public class EfRepository<TModel, TKey> : IEfRepository<TModel, TKey>
        where TModel : BaseEntity<TKey>
    {
        private readonly CoreDbContext _db;
        private readonly DbSet<TModel> _set;

  
        public EfRepository(CoreDbContext db)
        {
            _db = db;
            _set = db.Set<TModel>();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync() => await _set.ToListAsync();

        public async Task<TModel?> GetByIdAsync(TKey id) => await _set.FindAsync(id) as TModel;

        public async Task AddAsync(TModel entity)
        {
            await _set.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
     
        public async Task UpdateAsync(TModel entity)
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id, bool? IsConcreteDelete)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                if (IsConcreteDelete == null || IsConcreteDelete == true) { _set.Remove(entity); }
                //if stil exists
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    _db.Update(entity);
                }
                
                await _db.SaveChangesAsync();
            }
        }
             
    }
  }
