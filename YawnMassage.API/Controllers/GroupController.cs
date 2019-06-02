using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Api.Authorization;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Platform.Domain.Dto.Group;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // GET: api/Groups
        [HttpGet]
        [ClaimAuthorize("GROUP_SEARCH")]
        public async Task<PagedQueryResultSet<GroupDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]GroupSearchCriteria searchCriteria)
        {
            var data = await _groupService.GetAsync(gridCriteria, searchCriteria);
            return data;
        }

        // GET: api/Groups/guid
        [HttpGet("{id}")]
        [ClaimAuthorize("GROUP_UPDATE")]
        public async Task<GroupDto> Get(string id)
        {
            var group = await _groupService.GetAsync(id);
            return group;
        }

        // POST: api/Groups
        [HttpPost]
        [ClaimAuthorize("GROUP_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody] GroupDto dto)
        {
            var result = await _groupService.CreateAsync(dto);
            return result;
        }

        // PUT: api/Groups
        [HttpPut]
        [ClaimAuthorize("GROUP_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody] GroupDto dto)
        {
            var result = await _groupService.UpdateAsync(dto);
            return result;
        }

        // DELETE: api/Groups/guid
        [HttpDelete("{id}")]
        [ClaimAuthorize("GROUP_DELETE")]
        public async Task Delete(string id)
        {
            await _groupService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("GROUP_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _groupService.DeleteAsync(ids);
        }
    }
}
