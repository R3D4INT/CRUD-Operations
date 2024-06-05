using AutoMapper;
using backend.Dtos.Request;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ObjectResult> GetAllUsers()
        {
            try
            {
                var resultDto = await _userService.GetListByConditionAsync(e => e.Id != null);
                var result = _mapper.Map<IEnumerable<User>>(resultDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetUser/{id:int}")]
        public async Task<ObjectResult> GetUser(int id)
        {
            try
            {
                var resultDto = await _userService.GetSingleByConditionAsync(e => e.Id == id);
                var result = _mapper.Map<User>(resultDto);

                return Ok(result);

            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ObjectResult> CreateUser(User user)
        {
            try
            {
                var userDto = _mapper.Map<UserRequest>(user);
                await _userService.Add(userDto);
                return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUser/{id:int}")]
        public async Task<ObjectResult> Put(int id, User user)
        {
            try
            {
                var userDto = _mapper.Map<UserRequest>(user);
                await _userService.UpdateAsync(userDto, e => e.Id == id);
                
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{id:int}")]
        public async Task<ObjectResult> Delete(int id)
        {
            try
            {
                await _userService.Delete(e => e.Id == id);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
