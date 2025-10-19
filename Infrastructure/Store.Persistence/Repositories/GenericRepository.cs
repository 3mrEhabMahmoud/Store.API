using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Store.Domain.Contracts;
using Store.Domain.Entities;
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
            return changeTracker ?
                            await _context.Set<TEntity>().ToListAsync()
                            : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<TEntity?> GetAsync(Tkey key)
        {
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
    }
}
