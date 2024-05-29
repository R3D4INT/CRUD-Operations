using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using BackNewVersion.Models;
using BackNewVersionModels;


namespace BackNewVersion
{
    public interface IUserRepository
    {
        Task<Result<IEnumerable<User>>> GetListByConditionAsync(Expression<Func<User, bool>> condition);
        Task<Result<User>> GetSingleByConditionAsync(Expression<Func<User, bool>> condition);
        Task<Result<User>> AddAsync(User item);
        Task<Result<bool>> UpdateAsync(User item, Expression<Func<User, bool>> condition);
        Task<Result<bool>> DeleteAsync(Expression<Func<User, bool>> condition);
    }
}