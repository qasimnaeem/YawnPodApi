using YawnMassage.Platform.Domain.Constants;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Constants;
using YawnMassage.Platform.Domain.Dto.DataManagement;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto.Blob;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Exceptions;
using YawnMassage.Common.Services.Extensions;

namespace YawnMassage.Platform.Services
{
    public class BulkDataService : IBulkDataService
    {
        private readonly ISystemDataContext _systemDataContext;
        private readonly IApplicationService _applicationService;
        private readonly IPlatformServiceBusService _platformServiceBusService;
        private readonly RequestContext _requestContext;
        private readonly IGroupService _groupService;
        private readonly IBlobServiceFactory _blobServiceFactory;
        private readonly IUserService _userService;
        private readonly IUserContextService _userContextService;

        public BulkDataService(ISystemDataContext systemDataContext, IApplicationService applicationService, IGroupService groupService,
            RequestContext requestContext, IPlatformServiceBusService platformServiceBusService, IBlobServiceFactory blobServiceFactory,
            IUserService userService, IUserContextService userContextService)
        {
            _systemDataContext = systemDataContext;
            _applicationService = applicationService;
            _platformServiceBusService = platformServiceBusService;
            _requestContext = requestContext;
            _groupService = groupService;
            _blobServiceFactory = blobServiceFactory;
            _userService = userService;
            _userContextService = userContextService;
        }

        public async Task<PagedQueryResultSet<BulkDataOperationDto>> GetAsync(ResultSetCriteria gridCriteria)
        {
            var bulkJobList = await _systemDataContext.GetDocumentsWithPagingAsync<BulkDataOperation, BulkDataOperationDto>
                                (q => q.Where(b => b.RelatedUserId == _requestContext.UserId &&
                                b.RelatedGroupId == _requestContext.GroupId).Select(b => new BulkDataOperationDto()
                                {
                                    Id = b.Id,
                                    FileName = b.FileName,
                                    GroupId = b.RelatedGroupId,
                                    OperationTypeCode = b.OperationTypeCode,
                                    OperationStatusCode = b.OperationStatusCode,
                                    BlobReference = b.BlobReference,
                                    LogBlobReference = b.LogBlobReference,
                                    CreatedOnUtc = b.CreatedOnUtc,
                                }), gridCriteria);

            return bulkJobList;
        }

        public async Task<BulkDataOperation> GetAsync(string bulkDataId)
        {
            return await _systemDataContext.GetDocumentAsync<BulkDataOperation>(bulkDataId);
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetExportFileUrlAsync(string id)
        {
            var bulkData = await GetAsync(id);
            if (bulkData != null)
            {
                var downloadURL = await GetBulkDataFileDownloadUrlAsync(bulkData.BlobReference, BulkData.ExportsBasePath);
                return downloadURL;
            }
            return string.Empty;
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetImportFileUrlAsync(string id)
        {
            var bulkData = await GetAsync(id);
            if (bulkData != null)
            {
                var downloadURL = await GetBulkDataFileDownloadUrlAsync(bulkData.BlobReference, BulkData.ImportsBasePath);
                return downloadURL;
            }
            return string.Empty;
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetImportLogFileUrlAsync(string id)
        {
            var bulkData = await GetAsync(id);
            if (bulkData != null)
            {
                var downloadURL = await GetBulkDataFileDownloadUrlAsync(bulkData.LogBlobReference, BulkData.ImportsBasePath);
                return downloadURL;
            }
            return string.Empty;
        }

        [ExcludeFromCodeCoverage]
        public async Task<BlobDetailsDto> GetImportFileBlobDetails(string fileName)
        {
            var blobService = await _blobServiceFactory.CreateBlobServiceAsync(BulkData.BulkDataContainer, BulkData.ImportsBasePath);
            string blobReference = string.Concat(Guid.NewGuid(), "/", fileName);
            var sasToken = blobService.GenerateSasForBlob(blobReference, TimeSpan.FromMinutes(10), BlobSasPermissions.Create);
            var importUrl = sasToken.BlobUri + sasToken.SasToken;
            return new BlobDetailsDto
            {
                BlobUri = importUrl,
                BlobReference = blobReference
            };
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetImportFileDeleteUrl(string blobReference)
        {
            var blobService = await _blobServiceFactory.CreateBlobServiceAsync(BulkData.BulkDataContainer, BulkData.ImportsBasePath);
            var sasToken = blobService.GenerateSasForBlob(blobReference, TimeSpan.FromMinutes(10), BlobSasPermissions.Delete);
            var deleteUrl = sasToken.BlobUri + sasToken.SasToken;
            return deleteUrl;
        }

        public async Task UpdateAsync(BulkDataOperationDto bulkDataOperationDto)
        {
            var bulkData = await GetAsync(bulkDataOperationDto.Id);
            if (bulkData != null)
            {
                if (!string.IsNullOrWhiteSpace(bulkDataOperationDto.OperationStatusCode))
                    bulkData.OperationStatusCode = bulkDataOperationDto.OperationStatusCode;
                if (!string.IsNullOrWhiteSpace(bulkDataOperationDto.BlobReference))
                    bulkData.BlobReference = bulkDataOperationDto.BlobReference;
                if (!string.IsNullOrWhiteSpace(bulkDataOperationDto.LogBlobReference))
                    bulkData.LogBlobReference = bulkDataOperationDto.LogBlobReference;
            }

            await _systemDataContext.ReplaceDocumentAsync(bulkData);
        }

        public async Task CreateAsync(BulkDataOperationDto bulkDataOperationDto)
        {
            if (string.IsNullOrEmpty(_requestContext.GroupId))
            {
                throw new YawnMassageException("ERROR_GROUP_NOT_DEFINED");
            }

            var permittedGroups = await _applicationService.GetPermittedGroupListAsync();

            if (_requestContext.GroupId != "any" && _requestContext.GroupId != "*"
                && !permittedGroups.Where(c => c.Id == _requestContext.GroupId).Any())
            {
                throw new UnauthorizedAccessException("ERROR_UNAUTHORIZED");
            }

            if(bulkDataOperationDto.OperationTypeCode == BulkDataOperationType.Import 
                && bulkDataOperationDto.IsNewDatabaseFile)
            {
                if(bulkDataOperationDto.GroupId == "any")
                    throw new YawnMassageException("ERROR_ANY_GROUP_NEW_DATABASE_FILE");
                else if (! await _userContextService.HasPermissionAsync("BULKDATA_NEW_DATABASE_FILE"))
                {
                    throw new YawnMassageException("ERROR_NO_PERMISSION_NEW_DATABASE_FILE");
                }
            }

            var user = await _userService.GetUserAsync(_requestContext.UserId);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
            var userDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            var bulkDataOperation = new BulkDataOperation()
            {
                FileName = bulkDataOperationDto.OperationTypeCode == BulkDataOperations.ExportOperation ?
                    userDateTime.ToStandardReportNameFormatByDate(BulkDataFileDetails.ExportFileNamePrefix, BulkDataFileDetails.ExportFileExtension)
                    : bulkDataOperationDto.FileName,
                OperationStatusCode = BulkDataOperationStatus.Queued,
                OperationTypeCode = bulkDataOperationDto.OperationTypeCode,
                RelatedGroupId = bulkDataOperationDto.GroupId,
                RelatedUserId = _requestContext.UserId,
                BlobReference = bulkDataOperationDto.BlobReference,
                Remark = bulkDataOperationDto.Remark,
                IsNewDatabaseFile = bulkDataOperationDto.IsNewDatabaseFile
            };

            await _systemDataContext.CreateDocumentAsync(bulkDataOperation);

            if (bulkDataOperationDto.OperationTypeCode == BulkDataOperations.ExportOperation)
                await _platformServiceBusService.QueueBulkExportMessageAsync(bulkDataOperation.Id);
            else
                await _platformServiceBusService.QueueBulkImportMessageAsync(bulkDataOperation.Id);
        }

        private async Task<string> GetBulkDataFileDownloadUrlAsync(string blobReference, string basePath)
        {
            var blobService = await _blobServiceFactory.CreateBlobServiceAsync(BulkData.BulkDataContainer, basePath);
            var sasToken = blobService.GenerateSasForBlob(blobReference, TimeSpan.FromMinutes(10), BlobSasPermissions.Read);
            var downloadURL = sasToken.BlobUri + sasToken.SasToken;
            return downloadURL;
        }
    }
}
