using Microsoft.EntityFrameworkCore;
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Linq.Expressions;
            using System.Threading.Tasks;
            using OmniPulse.Entities.Common;
            using OmniPulse.DataAccess;

            namespace OmniPulse.DataAccess.Repositories;

            public class EfGenericRepository<T> : IGenericRepository<T> where T : BaseEntity
            {
                protected readonly OmniPulseDbContext _context;
                protected readonly DbSet<T> _dbSet;

                public EfGenericRepository(OmniPulseDbContext context)
                {
                    _context = context;
                    _dbSet = _context.Set<T>();
                }

                public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

                public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null) => 
                    predicate == null ? await _dbSet.ToListAsync() : await _dbSet.Where(predicate).ToListAsync();

                public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
                public void Update(T entity) => _dbSet.Update(entity);
                public void Delete(T entity) => _dbSet.Remove(entity);
            }