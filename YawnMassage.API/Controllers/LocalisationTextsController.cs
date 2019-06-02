using YawnMassage.Api.Authorization;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalisationTextsController : ControllerBase
    {
        private readonly ILocalisationService _localisationService;

        public LocalisationTextsController(ILocalisationService localisationService)
        {
            _localisationService = localisationService;
        }

        [HttpGet]
        [ClaimAuthorize("LOCALISATION_SEARCH")]
        public async Task<PagedQueryResultSet<LocalisationTextDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]ConfigurationSearchCriteria searchCriteria)
        {
            return await _localisationService.GetAsync(gridCriteria, searchCriteria);
        }

        [HttpGet("{id}")]
        [ClaimAuthorize("LOCALISATION_UPDATE")]
        public async Task<LocalisationTextDto> Get(string id)
        {
            return await _localisationService.GetAsync(id);
        }

        [HttpPost]
        [ClaimAuthorize("LOCALISATION_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody] LocalisationTextDto localisationTextDto)
        {
            return await _localisationService.CreateAsync(localisationTextDto);
        }

        [HttpPut]
        [ClaimAuthorize("LOCALISATION_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody] LocalisationTextDto localisationTextDto)
        {
            return await _localisationService.UpdateAsync(localisationTextDto);
        }

        [HttpDelete("{id}")]
        [ClaimAuthorize("LOCALISATION_DELETE")]
        public async Task Delete(string id)
        {
            await _localisationService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("LOCALISATION_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _localisationService.DeleteAsync(ids);
        }
    }
}

