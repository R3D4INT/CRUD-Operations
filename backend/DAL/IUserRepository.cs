using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using backend.Models;


namespace backend.DAL
{
    public interface IUserRepository
    {
        Task<Result<IEnumerable<UserDto>>> GetListByConditionAsync(Expression<Func<UserDto, bool>> condition);
        Task<Result<UserDto>> GetSingleByConditionAsync(Expression<Func<UserDto, bool>> condition);
        Task<Result<UserDto>> AddAsync(UserDto item);
        Task<Result<bool>> UpdateAsync(UserDto item, Expression<Func<UserDto, bool>> condition);
        Task<Result<bool>> DeleteAsync(Expression<Func<UserDto, bool>> condition);
    }
}