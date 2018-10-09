using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.SharedKernel;
using Demo.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Repository
{
    public abstract class BaseRepository<T, TId> : IRepository<T, TId> where T : Entity<TId>
    {
        protected internal DbContext Context;
        protected internal DbSet<T> DbSet;

        protected BaseRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            return DbSet.AsNoTracking().ToListAsync();
        }

        public async Task CreateOrUpdateAsync(T entity)
        {
            var existing=await DbSet.AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id.Equals(entity.Id));
            
            if (null !=existing) 
                Context.Entry(existing).CurrentValues.SetValues(entity);
            else
                await Context.AddAsync(entity);
        }

        public async Task RemoveAsync(TId id)
        {
            var entity = await DbSet.FindAsync(id);
            if (null != entity)
            {
                Context.Remove(entity);
            }
        }

        public Task SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}