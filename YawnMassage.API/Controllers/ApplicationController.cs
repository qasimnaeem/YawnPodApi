using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("GetUILookups")]
        public async Task<List<UILookupDto>> GetUILookups()
        {
            return await _applicationService.GetUILookupsForCurrentContextAsync();
        }

        [HttpGet("GetUILocalisations")]
        public async Task<List<UILocalisationTextDto>> GetUILocalisations()
        {
            return await _applicationService.GetUILocalisationTextsForCurrentContextAsync();
        }
        
        [HttpGet("GetConfigurations")]
        public async Task<List<ConfigurationDto>> GetConfigurations()
        {
            return await _applicationService.GetConfigurationsForCurrentContextAsync();
        }
        
        [HttpGet("GetPermissions")]
        public async Task<IEnumerable<string>> GetPermissions()
        {
            return await _applicationService.GetPermissionsForCurrentUserAsync();
        }

        [HttpGet("GetGroupList")]
       
        public async Task<List<ListItemDto>> GetGroupList()
        {
            return await _applicationService.GetPermittedGroupListAsync();
        }

        #region # Mobile Contexts #

        [HttpGet("getlookup/key/{key}")]
        public async Task<List<UILookupDto>> GetLookupAsync(string key)
        {
            return await _applicationService.GetLookupAsync(key);
        }

        //[HttpGet("getlocalisationtexts")]
        //public async Task<List<UILookupDto>> GetLocalisationTexts()
        //{
        //    return await _applicationService.GetUILocalisationTextsForMobileContextAsync();
        //}

        //[HttpGet("getcountrylookup")]
        //public async Task<List<UILookupDto>> GetCountryLookup()
        //{
        //    return await _applicationService.GetCountryLookupAsync();
        //}

        //[HttpGet("getmassagepurposelookup")]
        //public async Task<List<UILookupDto>> GetMassagePurposeLookup()
        //{
        //    return await _applicationService.GetMassagePurposeLookupAsync();
        //}

        #endregion
    }
}
