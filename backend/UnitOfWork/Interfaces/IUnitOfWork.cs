using backend.Repositories.Interfaces;

namespace backend.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ICountryRepository countryRepository { get; }
        IUserRepository userRepository { get; }
        Task SaveChangesAsync();
    }
}
