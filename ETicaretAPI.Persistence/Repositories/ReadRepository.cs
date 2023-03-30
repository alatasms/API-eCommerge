using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext context;

        public ReadRepository(ETicaretAPIDbContext context)
        {
            this.context = context;
        }

        public DbSet<T> Table => context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        => tracking ? Table : Table.AsNoTracking();

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        => tracking?await Table.FindAsync(Guid.Parse(id)):await Table.AsNoTracking().FirstOrDefaultAsync(p=>p.Id==Guid.Parse(id));

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        => tracking?await Table.FirstOrDefaultAsync(method):await Table.AsNoTracking().FirstOrDefaultAsync(method);

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        => tracking ? Table.Where(method) : Table.AsNoTracking().Where(method);
    }
}
