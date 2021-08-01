using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity:class
        where TContext:DbContext
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context)
        {
            this.Context = context;
        }

        
        // TODO : Turn async
        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public async virtual Task<TEntity> GetByIdAsync(int id)
        {
           return await Context.Set<TEntity>().FindAsync(id);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }


        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}

