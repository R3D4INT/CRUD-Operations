using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BackNewVersionModels;


namespace BackNewVersion
{
        [RoutePrefix("api/UserApi")]
        public class UserApiController : ApiController
        {
            private readonly IUserRepository _userRepository;
            public UserApiController(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            [HttpGet]
            [Route("GetAllUsers")]
            public async Task<IHttpActionResult> GetAllUsers()
            {
                var result = await _userRepository.GetListByConditionAsync(e => e.Id != null);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.ErrorMessage);
            }

            [HttpGet]
            [Route("GetUser/{id:int}")]
            public async Task<IHttpActionResult> GetUser(int id)
            {
                var result = await _userRepository.GetSingleByConditionAsync(e => e.Id == id);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return NotFound();
            }

            [HttpPost]
            [Route("CreateUser")]
            public async Task<IHttpActionResult> CreateUser(User user)
            {
                var result = await _userRepository.AddAsync(user);
                if (result.IsSuccess)
                {
                    return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
                }
                return BadRequest(result.ErrorMessage);
            }

            [HttpPut]
            [Route("UpdateUser/{id:int}")]
            public async Task<IHttpActionResult> Put(int id, User user)
            {
                var result = await _userRepository.UpdateAsync(user, e => e.Id == id);
                if (result.IsSuccess)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return BadRequest(result.ErrorMessage);
            }

            [HttpDelete]
            [Route("DeleteUser/{id:int}")]
            public async Task<IHttpActionResult> Delete(int id)
            {
                var result = await _userRepository.DeleteAsync(x => x.Id == id);
                if (result.IsSuccess)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return BadRequest(result.ErrorMessage);
            }
        }
}