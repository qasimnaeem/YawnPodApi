using YawnMassage.Api.Authorization;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Domain.Dto.ConfigurationSetting;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationManagementService;

        public ConfigurationController(IConfigurationService configurationManagementService)
        {
            _configurationManagementService = configurationManagementService;
        }

        [HttpGet]
        [ClaimAuthorize("CONFIGURATION_SEARCH")]
        public async Task<PagedQueryResultSet<ConfigurationSettingDto>> Get([FromQuery]ResultSetCriteria gridCriteria, [FromQuery]ConfigurationSearchCriteria searchCriteria)
        {
            return await _configurationManagementService.GetAsync(gridCriteria,searchCriteria);
        }

        [HttpGet("{id}")]
        [ClaimAuthorize("CONFIGURATION_UPDATE")]
        public async Task<ConfigurationSettingDto> Get(string id)
        {
            return await _configurationManagementService.GetAsync(id);
        }

        [HttpPut]
        [ClaimAuthorize("CONFIGURATION_UPDATE")]
        public async Task<DocumentUpdateResultDto> Put([FromBody]ConfigurationSettingDto configurationSettingDto)
        {
            return await _configurationManagementService.UpdateAsync(configurationSettingDto);
        }

        [HttpPost]
        [ClaimAuthorize("CONFIGURATION_NEW")]
        public async Task<DocumentUpdateResultDto> Post([FromBody]ConfigurationSettingDto configurationSettingDto)
        {
            return await _configurationManagementService.CreateAsync(configurationSettingDto);
        }

        [HttpDelete("{id}")]
        [ClaimAuthorize("CONFIGURATION_DELETE")]
        public async Task Delete(string id)
        {
            await _configurationManagementService.DeleteAsync(id);
        }

        [HttpDelete]
        [ClaimAuthorize("CONFIGURATION_BULKDELETE")]
        public async Task Delete([FromQuery(Name = "ids[]")] string[] ids)
        {
            await _configurationManagementService.DeleteAsync(ids);
        }

    }
}