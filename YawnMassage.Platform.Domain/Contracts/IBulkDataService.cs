using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto.Blob;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.DataManagement;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IBulkDataService" />
    /// </summary>
    public interface IBulkDataService
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="bulkDataId">The bulkDataId<see cref="string"/></param>
        /// <returns>The <see cref="Task{BulkDataOperation}"/></returns>
        Task<BulkDataOperation> GetAsync(string bulkDataId);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="bulkDataOperationDto">The bulkDataOperationDto<see cref="BulkDataOperationDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task UpdateAsync(BulkDataOperationDto bulkDataOperationDto);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{BulkDataOperationDto}}"/></returns>
        Task<PagedQueryResultSet<BulkDataOperationDto>> GetAsync(ResultSetCriteria gridCriteria);

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="bulkDataOperationDto">The bulkDataOperationDto<see cref="BulkDataOperationDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task CreateAsync(BulkDataOperationDto bulkDataOperationDto);

        /// <summary>
        /// The GetExportFileUrlAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetExportFileUrlAsync(string id);

        /// <summary>
        /// The GetImportFileUrlAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetImportFileUrlAsync(string id);

        /// <summary>
        /// The GetImportLogFileUrlAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetImportLogFileUrlAsync(string id);

        /// <summary>
        /// The GetImportFileBlobDetails
        /// </summary>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        /// <returns>The <see cref="Task{BlobDetailsDto}"/></returns>
        Task<BlobDetailsDto> GetImportFileBlobDetails(string fileName);

        /// <summary>
        /// The GetImportFileDeleteUrl
        /// </summary>
        /// <param name="blobReference">The blobReference<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetImportFileDeleteUrl(string blobReference);
    }
}
