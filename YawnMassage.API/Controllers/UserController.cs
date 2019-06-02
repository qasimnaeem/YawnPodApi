using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using YawnMassage.Api.Authorization;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Identity;
using YawnMassage.Platform.Domain.Dto.User;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [ClaimAuthorize("USER_SEARCH")]
        public async Task<PagedQueryResultSet<UserDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]UserSearchCriteria searchCriteria)
        {
            var data = await _userService.GetAsync(gridCriteria, searchCriteria);
            return data;
        }

        // GET: api/Users/guid
        [HttpGet("{id}")]
        [ClaimAuthorize("any:USER_UPDATE")]
        public async Task<UserDto> Get(string id)
        {
            var user = await _userService.GetUserForEditAsync(id);
            return user;
        }

        // POST: api/Users
        [HttpPost]
        [ClaimAuthorize("any:USER_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody] UserDto dto)
        {
            var result = await _userService.CreateAsync(dto);
            return result;
        }

        // PUT: api/Users
        [HttpPut]
        [ClaimAuthorize("any:USER_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody] UserDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            return result;
        }

        // DELETE: api/Users/guid
        [HttpDelete("{id}")]
        [ClaimAuthorize("USER_DELETE")]
        public async Task Delete(string id)
        {
            await _userService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("USER_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _userService.DeleteAsync(ids);
        }

        [HttpPost("ActivateUser")]
        [ClaimAuthorize("any:USER_ACTIVATE")]
        public async Task<DocumentUpdateResultDto> ActivateUser([FromBody] UserActivateDto userActivateDto)
        {
            var result = await _userService.ActivateUserAsync(userActivateDto.UserId);
            return result;
        }

        [HttpGet("getPostAuthDataAsync/{mobileNumber}")]
        public async Task<MobileAuthResultDto> GetPostAuthDataAsync(string mobileNumber)
        {           
            return await _userService.GetPostAuthDataAsync(mobileNumber);
        }       
    }
}