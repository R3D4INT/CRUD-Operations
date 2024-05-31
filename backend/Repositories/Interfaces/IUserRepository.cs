using System.Linq.Expressions;
using backend.Dtos.Request;
using backend.Models;

namespace backend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<IEnumerable<UserRequest>>> GetListByConditionAsync(Expression<Func<UserRequest, bool>> condition);
        Task<Result<UserRequest>> GetSingleByConditionAsync(Expression<Func<UserRequest, bool>> condition);
        Task<Result<UserRequest>> AddAsync(UserRequest item);
        Task<Result<bool>> UpdateAsync(UserRequest item, Expression<Func<UserRequest, bool>> condition);
        Task<Result<bool>> DeleteAsync(Expression<Func<UserRequest, bool>> condition);
    }
}