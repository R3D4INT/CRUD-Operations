using System.Linq.Expressions;
using AutoMapper;
using backend.DAL;
using backend.Dtos.Request;
using backend.Helpers;
using backend.Helpers.Messages;
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
            catch (ArgumentNullException ex)
            {
                return Result<T>.Fail(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<T>.Fail(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return Result<T>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return Result<T>.Fail(errorMessage);
            }
        }

        public async Task<Result<IEnumerable<UserRequest>>> GetListByConditionAsync(Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var items = await _context.Set<User>().Include(u => u.Country).Where(entityCondition).ToListAsync();
                
                return _mapper.Map<IEnumerable<UserRequest>>(items);
            }, RepositoryMessages.FailedGetListOfEntityMessage);
        }

        public async Task<Result<UserRequest>> GetSingleByConditionAsync(Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var item = await _context.Set<User>().Include(u => u.Country).FirstOrDefaultAsync(entityCondition);

                return _mapper.Map<UserRequest>(item);
            }, RepositoryMessages.FailedGetSingleItemMessage);
        }

        public async Task<Result<UserRequest>> AddAsync(UserRequest item)
        {
            return await PerformOperationAsync(async () =>
            {
                var user = _mapper.Map<User>(item);
                var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == item.Country.Name);
                user.Country = country;
                _context.Set<User>().Add(user);

                return item;
            }, RepositoryMessages.FailedAddItemMessage);
        }

        public async Task<Result<bool>> UpdateAsync(UserRequest item, Expression<Func<UserRequest, bool>> condition)
        {
            return await PerformOperationAsync(async () =>
            {
                var entityCondition = _mapper.Map<Expression<Func<User, bool>>>(condition);
                var itemToUpdate = await _context.Set<User>().Include(u => u.Country).FirstOrDefaultAsync(entityCondition);

                if (itemToUpdate == null)
                {
                    throw new EntityNotFoundException(RepositoryMessages.FailedUpdateItemMessage);
                }

                _context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                var country = await _context.Countries.FirstAsync(e => e.Name == item.Country.Name);
                if (country != null)
                {
                    itemToUpdate.Country = country;
                }
                else
                {
                    itemToUpdate.Country = item.Country; 
                    _context.Entry(itemToUpdate.Country).State = EntityState.Added;
                }

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

                return true;
            }, RepositoryMessages.FailedDeleteItemMessage);
        }

        public async Task<Result<bool>> DeleteUsersOlderThan30Async()
        {
            return await PerformOperationAsync(async () =>
            {
                var usersToRemove = await _context.Set<User>().Where(u => u.Age > 30).ToListAsync();

                if (!usersToRemove.Any())
                {
                    throw new EntityNotFoundException(RepositoryMessages.NoUsersOlderThanThirty);
                }

                _context.Set<User>().RemoveRange(usersToRemove);

                return true;
            }, RepositoryMessages.NoUsersOlderThanThirty);
        }
    }
}