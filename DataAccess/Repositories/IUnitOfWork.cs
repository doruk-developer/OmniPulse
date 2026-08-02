using System;
            using System.Threading.Tasks;
            using OmniPulse.Entities.Common;

            namespace OmniPulse.DataAccess.Repositories;

            public interface IUnitOfWork : IDisposable
            {
                IGenericRepository<T> Repository<T>() where T : BaseEntity;
                Task<int> SaveChangesAsync();
            }