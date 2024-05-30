using Microsoft.AspNetCore.Http;

using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserApiController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GetAllUsers")]
        public async Task<ObjectResult> GetAllUsers()
        {
            var result = await _userRepository.GetListByConditionAsync(e => e.Id != null);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GetUser/{id:int}")]
        public async Task<ObjectResult> GetUser(int id)
        {
            var result = await _userRepository.GetSingleByConditionAsync(e => e.Id == id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("CreateUser")]
        public async Task<ObjectResult> CreateUser(User user)
        {
            var result = await _userRepository.AddAsync(user);
            if (result.IsSuccess)
            {
                return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("UpdateUser/{id:int}")]
        public async Task<ObjectResult> Put(int id, User user)
        {
            var result = await _userRepository.UpdateAsync(user, e => e.Id == id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("DeleteUser/{id:int}")]
        public async Task<ObjectResult> Delete(int id)
        {
            var result = await _userRepository.DeleteAsync(x => x.Id == id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
