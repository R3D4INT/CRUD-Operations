using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using backend.Helpers;
using backend.Models;
using Microsoft.EntityFrameworkCore;


namespace backend.DAL
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

        public async Task<Result<IEnumerable<UserDto>>> GetListByConditionAsync(Expression<Func<UserDto, bool>> condition)
        {
            return await PerformOperationAsync<IEnumerable<UserDto>>(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var items = await _context.Set<User>().Where(entityCondition).ToListAsync();
                return _mapper.Map<IEnumerable<UserDto>>(items);
            }, RepositoryMessages.FailedGetListOfEntityMessage);
        }

        public async Task<Result<UserDto>> GetSingleByConditionAsync(Expression<Func<UserDto, bool>> condition)
        {
            return await PerformOperationAsync<UserDto>(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var item = await _context.Set<User>().FirstOrDefaultAsync(entityCondition);
                return _mapper.Map<UserDto>(item);
            }, RepositoryMessages.FailedGetSingleItemMessage);
        }

        public async Task<Result<UserDto>> AddAsync(UserDto item)
        {
            return await PerformOperationAsync<UserDto>(async () =>
            {
                var user = _mapper.Map<User>(item);
                _context.Set<User>().Add(user);
                await _context.SaveChangesAsync();
                return item;
            }, RepositoryMessages.FailedAddItemMessage);
        }

        public async Task<Result<bool>> UpdateAsync(UserDto item, Expression<Func<UserDto, bool>> condition)
        {
            return await PerformOperationAsync<bool>(async () =>
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

        public async Task<Result<bool>> DeleteAsync(Expression<Func<UserDto, bool>> condition)
        {
            return await PerformOperationAsync<bool>(async () =>
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