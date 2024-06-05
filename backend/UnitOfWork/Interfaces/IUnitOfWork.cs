using backend.Repositories.Interfaces;

namespace backend.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        Task SaveChangesAsync();
    }
}
