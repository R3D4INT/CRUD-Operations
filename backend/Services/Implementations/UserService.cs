using System.Linq.Expressions;
using AutoMapper;
using backend.Dtos.Request;
using backend.Helpers.Messages;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using backend.UnitOfWork.Interfaces;

namespace backend.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<T> ExecuteRepositoryMethod<T>(Func<IUserRepository, Task<Result<T>>> method, string errorMessage)
        {
            var result = await method(_unitOfWork.userRepository);

            if (!result.IsSuccess)
            {
                throw new Exception(errorMessage);
            }

            await _unitOfWork.SaveChangesAsync();

            return result.Data;
        }

        public virtual async Task AddAsync(UserRequest user)
        {
            await ExecuteRepositoryMethod(repo => repo.AddAsync(user), $"{ServiceMessages.FailedToAdd} {nameof(User)}");
        }

        public virtual async Task DeleteAsync(Expression<Func<UserRequest, bool>> predicate)
        {
            await ExecuteRepositoryMethod(repo => repo.DeleteAsync(predicate), $"{ServiceMessages.FailedToDelete} {nameof(User)}.");
        }

        public virtual async Task<IEnumerable<UserRequest>> GetListByConditionAsync(Expression<Func<UserRequest, bool>> predicate)
        {
            return await ExecuteRepositoryMethod(repo => repo.GetListByConditionAsync(predicate), $"{ServiceMessages.FailedGetList} {nameof(User)}s.");
        }

        public async Task<UserRequest> GetSingleByConditionAsync(Expression<Func<UserRequest, bool>> predicate)
        {
            return await ExecuteRepositoryMethod(repo => repo.GetSingleByConditionAsync(predicate), $"{ServiceMessages.FailedGetSingle} {nameof(User)}.");
        }

        public virtual async Task UpdateAsync(UserRequest user, Expression<Func<UserRequest, bool>> condition)
        {
            await ExecuteRepositoryMethod(repo => repo.UpdateAsync(user, condition), $"{ServiceMessages.FailedToUpdate} {nameof(User)} {ServiceMessages.WithId} {user.Id}.");
        }

        public virtual async Task DeleteUsersOlderThanThirty()
        {
            await ExecuteRepositoryMethod(repo => repo.DeleteUsersOlderThan30Async(), $"{ServiceMessages.FailedToDeleteUsersOlderThanThirty} {nameof(User)}");
        }
    }
}
