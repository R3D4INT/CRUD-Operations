using System.Linq.Expressions;
using AutoMapper;
using backend.DAL;
using backend.Helpers;
using backend.Models;

namespace backend.Services
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
        public virtual async Task Add(UserDto obj)
        {
            await ExecuteRepositoryMethod(repo => repo.AddAsync(obj), $"{ServiceMessages.FailedToAdd} {typeof(User).Name}");
        }
        
        public virtual async Task Delete(Expression<Func<UserDto, bool>> predicate)
        {
            await ExecuteRepositoryMethod(repo => repo.DeleteAsync(predicate), $"{ServiceMessages.FailedToDelete} {typeof(User).Name}.");
        }

        public virtual async Task<IEnumerable<UserDto>> GetListByCondition(Expression<Func<UserDto, bool>> predicate)
        {
            return await ExecuteRepositoryMethod(repo => repo.GetListByConditionAsync(predicate), $"{ServiceMessages.FailedGetList} {typeof(User).Name}s.");
        }

        public async Task<UserDto> GetSingleByCondition(Expression<Func<UserDto, bool>> predicate)
        {
            return await ExecuteRepositoryMethod(repo => repo.GetSingleByConditionAsync(predicate), $"{ServiceMessages.FailedGetSingle} {typeof(User).Name}.");
        }

        public virtual async Task Update(UserDto obj, Expression<Func<UserDto, bool>> condition)
        {
            await ExecuteRepositoryMethod(repo => repo.UpdateAsync(obj, condition), $"{ServiceMessages.FailedToUpdate} {typeof(User).Name} {ServiceMessages.WithId} {obj.Id}.");
        }
    }
}
