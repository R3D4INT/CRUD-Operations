using System.Linq.Expressions;
using backend.DAL;

namespace backend.Services;

public interface IUserService
{
    Task<UserDto> GetSingleByCondition(Expression<Func<UserDto, bool>> predicate);
    Task<IEnumerable<UserDto>> GetListByCondition(Expression<Func<UserDto, bool>> predicate);
    Task Add(UserDto obj);
    Task Update(UserDto obj, Expression<Func<UserDto, bool>> condition);
    Task Delete(Expression<Func<UserDto, bool>> predicate);
}