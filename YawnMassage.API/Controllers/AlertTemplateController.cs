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
    public class AlertTemplateController : ControllerBase
    {
        private readonly IAlertTemplateService _alertTemplateService;

        public AlertTemplateController(IAlertTemplateService alertTemplateService)
        {
            _alertTemplateService = alertTemplateService;
        }

        [HttpGet]
        [ClaimAuthorize("TEMPLATE_SEARCH")]
        public async Task<PagedQueryResultSet<AlertTemplateDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]AlertTemplateSearchCriteria searchCriteria)
        {
            return await _alertTemplateService.GetAsync(gridCriteria, searchCriteria);
        }

        [HttpGet("{id}")]
        [ClaimAuthorize("TEMPLATE_UPDATE")]
        public async Task<AlertTemplateDto> Get(string id)
        {
            return await _alertTemplateService.GetAsync(id);
        }

        [HttpPost]
        [ClaimAuthorize("TEMPLATE_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody] AlertTemplateDto alertTemplateDto)
        {
            return await _alertTemplateService.CreateAsync(alertTemplateDto);
        }

        [HttpPut]
        [ClaimAuthorize("TEMPLATE_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody] AlertTemplateDto alertTemplateDto)
        {
            return await _alertTemplateService.UpdateAsync(alertTemplateDto);
        }

        [HttpDelete("{id}")]
        [ClaimAuthorize("TEMPLATE_DELETE")]
        public async Task Delete(string id)
        {
            await _alertTemplateService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("TEMPLATE_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _alertTemplateService.DeleteAsync(ids);
        }
    }
}

