using YawnMassage.Api.Authorization;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupsController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        // GET: api/Lookups
        [HttpGet]
        [ClaimAuthorize("LOOKUP_SEARCH")]
        public async Task<PagedQueryResultSet<LookupDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]ConfigurationSearchCriteria searchCriteria)
        {
            var  data =  await _lookupService.GetLookupListAsync(gridCriteria, searchCriteria);
            return data;
        }

        // GET: api/Lookups/guid
        [HttpGet("{id}")]
        [ClaimAuthorize("LOOKUP_UPDATE")]
        public async Task<LookupDto> Get(string id)
        {
            var lookup = await _lookupService.GetAsync(id);
            return lookup;
        }

        // POST: api/Lookups
        [HttpPost]
        [ClaimAuthorize("LOOKUP_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody] LookupDto dto)
        {
            var result = await _lookupService.CreateAsync(dto);
            return result;
        }

        // PUT: api/Lookups
        [HttpPut]
        [ClaimAuthorize("LOOKUP_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody] LookupDto dto)
        {
            var result = await _lookupService.UpdateAsync(dto);
            return result;
        }

        // DELETE: api/Lookups/guid
        [HttpDelete("{id}")]
        [ClaimAuthorize("LOOKUP_DELETE")]
        public async Task Delete(string id)
        {
            await _lookupService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("LOOKUP_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _lookupService.DeleteAsync(ids);
        }
    }
}
