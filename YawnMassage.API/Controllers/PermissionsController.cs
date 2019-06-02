using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YawnMassage.Api.Authorization;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService )
        {
            _permissionService = permissionService;
        }

        // GET: api/Permissions
        [HttpGet]
        [ClaimAuthorize("PERMISSION_SEARCH")]
        public async Task<PagedQueryResultSet<PermissionConfigDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]PermissionSearchCriteria searchCriteria)
        {
            var data = await _permissionService.GetAsync(gridCriteria, searchCriteria);
            return data;
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        [ClaimAuthorize("PERMISSION_UPDATE")]
        public async Task<PermissionConfigDto> Get(string id)
        {
            var data = await _permissionService.GetAsync(id);
            return data;
        }

        // POST: api/Permissions
        [HttpPost]
        [ClaimAuthorize("PERMISSION_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody] PermissionConfigDto dto)
        {
            var result = await _permissionService.CreateAsync(dto);
            return result;
        }

        // PUT: api/Permissions/5
        [HttpPut]
        [ClaimAuthorize("PERMISSION_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody] PermissionConfigDto dto)
        {
            var result = await _permissionService.UpdateAsync(dto);
            return result;
        }

        // DELETE: api/Lookups/guid
        [HttpDelete("{id}")]
        [ClaimAuthorize("PERMISSION_DELETE")]
        public async Task Delete(string id)
        {
            await _permissionService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("PERMISSION_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _permissionService.DeleteAsync(ids);
        }
    }
}
