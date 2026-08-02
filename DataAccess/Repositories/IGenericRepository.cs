using System;
            using System.Collections.Generic;
            using System.Linq.Expressions;
            using System.Threading.Tasks;
            using OmniPulse.Entities.Common;

            namespace OmniPulse.DataAccess.Repositories;

            public interface IGenericRepository<T> where T : BaseEntity
            {
                Task<T?> GetByIdAsync(int id);
                Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
                Task AddAsync(T entity);
                void Update(T entity);
                void Delete(T entity);
            }