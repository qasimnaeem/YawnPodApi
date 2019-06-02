using YawnMassage.Api.Authorization;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.DataManagement;
using YawnMassage.Common.Domain.Dto.Blob;
using YawnMassage.Common.Domain.Dto.ResultSet;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkDataController : ControllerBase
    {
        private readonly IBulkDataService _bulkDataService;
        public BulkDataController(IBulkDataService bulkDataService)
        {
            _bulkDataService = bulkDataService;
        }

        [HttpGet]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task<PagedQueryResultSet<BulkDataOperationDto>> Get([FromQuery]ResultSetCriteria gridCriteria)
        {
            return await _bulkDataService.GetAsync(gridCriteria);
        }

        [HttpPost]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task Post([FromBody]BulkDataOperationDto bulkDataOperationDto)
        {
            await _bulkDataService.CreateAsync(bulkDataOperationDto);
        }

        [HttpGet("GetExportFileURL")]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task<string> GetExportFileUrl(string id)
        {
            return await _bulkDataService.GetExportFileUrlAsync(id);
        }

        [HttpGet("GetImportFileURL")]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task<string> GetImportFileUrl(string id)
        {
            return await _bulkDataService.GetImportFileUrlAsync(id);
        }

        [HttpGet("GetImportLogFileURL")]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task<string> GetImportLogFileUrl(string id)
        {
            return await _bulkDataService.GetImportLogFileUrlAsync(id);
        }

        [HttpGet("GetImportFileBlobDetails")]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task<BlobDetailsDto> GetImportFileBlobDetails(string fileName)
        {
            return await _bulkDataService.GetImportFileBlobDetails(fileName);
        }

        [HttpGet("GetFileDeleteUrl")]
        [ClaimAuthorize("any:NAV_BULKDATA")]
        public async Task<string> GetFileDeleteUrl(string blobReference)
        {
            return await _bulkDataService.GetImportFileDeleteUrl(blobReference);
        }
    }
}