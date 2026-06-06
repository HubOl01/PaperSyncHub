using System.Collections.Generic;
using System.Threading.Tasks;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositories;


public abstract class RepositoryBase<T> : IRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;

    protected RepositoryBase(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}