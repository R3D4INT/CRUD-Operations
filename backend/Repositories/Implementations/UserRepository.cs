using System.Linq.Expressions;
using AutoMapper;
using backend.DAL;
using backend.Dtos.Request;
using backend.Helpers;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private async Task<Result<T>> PerformOperationAsync<T>(Func<Task<T>> operation, string errorMessage)
        {
            try
            {
                var result = await operation();
                return Result<T>.Success(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Result<T>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<T>.Fail(errorMessage);
            }
        }

        public async Task<Result<IEnumerable<UserRequest>>> GetListByConditionAsync(Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var items = await _context.Set<User>().Where(entityCondition).ToListAsync();
                return _mapper.Map<IEnumerable<UserRequest>>(items);
            }, RepositoryMessages.FailedGetListOfEntityMessage);
        }

        public async Task<Result<UserRequest>> GetSingleByConditionAsync(Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var item = await _context.Set<User>().FirstOrDefaultAsync(entityCondition);
                return _mapper.Map<UserRequest>(item);
            }, RepositoryMessages.FailedGetSingleItemMessage);
        }

        public async Task<Result<UserRequest>> AddAsync(UserRequest item)
        {
            return await PerformOperationAsync(async () =>
            {
                var user = _mapper.Map<User>(item);
                _context.Set<User>().Add(user);
                await _context.SaveChangesAsync();
                return item;
            }, RepositoryMessages.FailedAddItemMessage);
        }

        public async Task<Result<bool>> UpdateAsync(UserRequest item, Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var itemToUpdate = await _context.Set<User>().FirstOrDefaultAsync(entityCondition);

                if (itemToUpdate == null)
                {
                    throw new EntityNotFoundException(RepositoryMessages.FailedUpdateItemMessage);
                }

                _context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();

                return true;
            }, RepositoryMessages.FailedUpdateItemMessage);
        }

        public async Task<Result<bool>> DeleteAsync(Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var itemsToRemove = await _context.Set<User>().Where(entityCondition).ToListAsync();

                if (!itemsToRemove.Any())
                {
                    throw new EntityNotFoundException(RepositoryMessages.FailedDeleteItemMessage);
                }

                _context.Set<User>().RemoveRange(itemsToRemove);
                await _context.SaveChangesAsync();

                return true;
            }, RepositoryMessages.FailedDeleteItemMessage);
        }
    }
}