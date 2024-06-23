using backend.Helpers;
using backend.Services.Interfaces;
using Quartz;

namespace backend.BackGroundJob
{
    public class ClearOldUsersInDatabaseJob : IJob
    {
        private readonly IUserService _userService;

        public ClearOldUsersInDatabaseJob(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Execute(IJobExecutionContext context){
            try
            {
                await _userService.DeleteUsersOlderThanThirty();
            }
            catch (Exception ex)
            {
                throw new Exception($"{JobStrings.NoUsersOlderThanThirty} {ex.Message}");
            }
        }
    }   
}
