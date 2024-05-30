using System.Collections;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using backend.DAL;
using backend.Services;

namespace backend.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
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

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GetAllUsers")]
        public async Task<ObjectResult> GetAllUsers()
        {
            try
            {
                var resultDto = await _userService.GetListByCondition(e => e.Id != null);
                var result = _mapper.Map<IEnumerable<User>>(resultDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GetUser/{id:int}")]
        public async Task<ObjectResult> GetUser(int id)
        {
            try
            {
                var resultDto = await _userService.GetSingleByCondition(e => e.Id == id);
                var result = _mapper.Map<User>(resultDto);

                return Ok(result);

            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("CreateUser")]
        public async Task<ObjectResult> CreateUser(User user)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(user);
                await _userService.Add(userDto);
                return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("UpdateUser/{id:int}")]
        public async Task<ObjectResult> Put(int id, User user)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(user);
                await _userService.Update(userDto, e => e.Id == id);
                
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("DeleteUser/{id:int}")]
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
