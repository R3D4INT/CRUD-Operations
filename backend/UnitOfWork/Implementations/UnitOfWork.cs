using backend.DAL;
using backend.Repositories.Interfaces;
using backend.UnitOfWork.Interfaces;

namespace backend.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDBContext _dbContext;
        public IUserRepository userRepository { get; private set; }
        public ICountryRepository countryRepository { get; private set; }
        private bool disposed = false;

        public UnitOfWork(AppDBContext dbContext, IUserRepository userRepository, ICountryRepository countryRepository)
        {
            _dbContext = dbContext;
            this.userRepository = userRepository;
            this.countryRepository = countryRepository;
        }

        public Task SaveChangesAsync()
        {
            _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
