using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using backend;
using backend.Models;
using Microsoft.EntityFrameworkCore;


namespace backend
{
    public class UserRepository : IUserRepository
    {
        public async Task<Result<IEnumerable<User>>> GetListByConditionAsync(Expression<Func<User, bool>> condition)
        {
            try
            {
                using (var context = new AppDBContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    var items = await context.Set<User>().Where(condition).ToListAsync();
                    return Result<IEnumerable<User>>.Success(items);
                }
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Fail("Error retrieving data.");
            }
        }
        public async Task<Result<User>> GetSingleByConditionAsync(Expression<Func<User, bool>> condition)
        {
            using (var context = new AppDBContext())
            {
                await context.Database.EnsureCreatedAsync();
                var query = context.Set<User>().AsQueryable();

                var item = await query.FirstOrDefaultAsync(condition);

                if (item != null)
                {
                    return Result<User>.Success(item);
                }
                else
                {
                    return Result<User>.Fail("Item not found.");
                }
            }
        }
        public async Task<Result<User>> AddAsync(User item)
        {
            try
            {
                using (var context = new AppDBContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    context.Set<User>().Add(item);

                    await context.SaveChangesAsync();

                    return Result<User>.Success(item);
                }
            }
            catch (Exception ex)
            {
                return Result<User>.Fail("Error adding data.");
            }
        }
        public async Task<Result<bool>> UpdateAsync(User item, Expression<Func<User, bool>> condition)
        {
            try
            {
                using (var context = new AppDBContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    var itemToUpdate = await context.Set<User>().FirstOrDefaultAsync(condition);

                    if (itemToUpdate == null)
                    {
                        return Result<bool>.Fail("Item not found.");
                    }

                    context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                    await context.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Error updating data.");
            }
        }
        public async Task<Result<bool>> DeleteAsync(Expression<Func<User, bool>> condition)
        {
            try
            {
                using (var context = new AppDBContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    var itemsToRemove = await context.Set<User>().Where(condition).ToListAsync();

                    if (!itemsToRemove.Any())
                    {
                        return Result<bool>.Fail("Item not found.");
                    }
                    context.Set<User>().RemoveRange(itemsToRemove);
                    await context.SaveChangesAsync();

                    return Result<bool>.Success(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Error deleting data.");
            }
        }
    }
}