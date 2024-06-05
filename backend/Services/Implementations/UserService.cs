using System.Linq.Expressions;
using AutoMapper;
using backend.Dtos.Request;
using backend.Helpers;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private async Task<T> ExecuteRepositoryMethod<T>(Func<IUserRepository, Task<Result<T>>> method, string errorMessage)
        {
            var result = await method(_repository);

            if (!result.IsSuccess)
            {
                throw new Exception(errorMessage);
            }

            return _mapper.Map<T>(result.Data);
        }

        public virtual async Task Add(UserRequest user)
        {
            await ExecuteRepositoryMethod(repo => repo.AddAsync(user), $"{ServiceMessages.FailedToAdd} {typeof(User).Name}");
        }

        public virtual async Task Delete(Expression<Func<UserRequest, bool>> predicate)
        {
            await ExecuteRepositoryMethod(repo => repo.DeleteAsync(predicate), $"{ServiceMessages.FailedToDelete} {typeof(User).Name}.");
        }

        public virtual async Task<IEnumerable<UserRequest>> GetListByConditionAsync(Expression<Func<UserRequest, bool>> predicate)
        {
            return await ExecuteRepositoryMethod(repo => repo.GetListByConditionAsync(predicate), $"{ServiceMessages.FailedGetList} {typeof(User).Name}s.");
        }

        public async Task<UserRequest> GetSingleByConditionAsync(Expression<Func<UserRequest, bool>> predicate)
        {
            return await ExecuteRepositoryMethod(repo => repo.GetSingleByConditionAsync(predicate), $"{ServiceMessages.FailedGetSingle} {typeof(User).Name}.");
        }

        public virtual async Task UpdateAsync(UserRequest user, Expression<Func<UserRequest, bool>> condition)
        {
            await ExecuteRepositoryMethod(repo => repo.UpdateAsync(user, condition), $"{ServiceMessages.FailedToUpdate} {typeof(User).Name} {ServiceMessages.WithId} {user.Id}.");
        }
    }
}
