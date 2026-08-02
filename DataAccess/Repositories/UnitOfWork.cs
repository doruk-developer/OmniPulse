using System;
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using OmniPulse.Entities.Common;
            using OmniPulse.DataAccess;

            namespace OmniPulse.DataAccess.Repositories;

            public class UnitOfWork : IUnitOfWork
            {
                private readonly OmniPulseDbContext _context;
                private Dictionary<Type, object> _repositories = new();

                public UnitOfWork(OmniPulseDbContext context) => _context = context;

                public IGenericRepository<T> Repository<T>() where T : BaseEntity
                {
                    var type = typeof(T);
                    if (!_repositories.ContainsKey(type))
                    {
                        _repositories.Add(type, Activator.CreateInstance(typeof(EfGenericRepository<>).MakeGenericType(type), _context)!);
                    }
                    return (IGenericRepository<T>)_repositories[type];
                }

                public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
                public void Dispose() => _context.Dispose();
            }