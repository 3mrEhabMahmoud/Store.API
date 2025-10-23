using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Store.Domain.Contracts;
using Store.Domain.Entities;
using Store.Domain.Entities.Products;
using Store.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Repositories
{

    public class GenericRepository<Tkey, TEntity>(StoreDbContext _context) : IGenericRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool changeTracker = false)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return changeTracker ?
                    await _context.Products.Include(p => p.Brand).Include(p => p.Type).ToListAsync() as IEnumerable<TEntity>
                    : await _context.Products.Include(p => p.Brand).Include(p => p.Type).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;

            }
            return changeTracker ?
                            await _context.Set<TEntity>().ToListAsync()
                            : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Tkey key)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync(p => p.Id == key as int?) as TEntity;
            
            }
            return await _context.Set<TEntity>().FindAsync(key);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);

        }
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<Tkey,TEntity> spec ,bool changeTracker = false)
        {
            return await ApplySpecifications(spec).ToListAsync();

        }
        public async Task<TEntity?> GetAsync(ISpecifications<Tkey, TEntity> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();

        }
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<Tkey,TEntity> spec)
        {
            return SpecificationsEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }

    }
}
