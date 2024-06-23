using System.Linq.Expressions;
using backend.Dtos.Request;

namespace backend.Services.Interfaces;

public interface IUserService
{
    Task<UserRequest> GetSingleByConditionAsync(Expression<Func<UserRequest, bool>> predicate);
    Task<IEnumerable<UserRequest>> GetListByConditionAsync(Expression<Func<UserRequest, bool>> predicate);
    Task AddAsync(UserRequest user);
    Task UpdateAsync(UserRequest user, Expression<Func<UserRequest, bool>> condition);
    Task DeleteAsync(Expression<Func<UserRequest, bool>> predicate);
    Task DeleteUsersOlderThanThirty();
}