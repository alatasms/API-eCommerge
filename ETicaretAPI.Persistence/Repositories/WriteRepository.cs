using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        public readonly ETicaretAPIDbContext _context;

        public WriteRepository(ETicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table =>_context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
           EntityEntry<T> entityEntry= await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }
        public async Task<bool> AddRangeAsync(List<T> data)
        {
           await Table.AddRangeAsync(data);
           return true;
        }
        public bool Remove(T model)
        {
           EntityEntry<T> entityEntry = Table.Remove(model);
           return entityEntry.State==EntityState.Deleted;
        }
        public async Task<bool> RemoveAsync(string id)
        {
            T model = await Table.FindAsync(Guid.Parse(id));
             return Remove(model);
            
        }
        public bool RemoveRange(List<T> data)
        {
            Table.RemoveRange(data);
            return true;
        } 
        public bool Update(T model)
        {
            EntityEntry<T> entityEntry= Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync()=> await _context.SaveChangesAsync();

       
    }
}
