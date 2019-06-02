using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Domain.Dto.User;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IPlatformServiceBusService" />
    /// </summary>
    public interface IPlatformServiceBusService
    {
        /// <summary>
        /// The TriggerPodAccessDefinitionGenerationAsync
        /// </summary>
        /// <param name="userUpdateMessageDto">The userUpdateMessageDto<see cref="UserUpdateMessageDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task TriggerPodAccessDefinitionGenerationAsync(UserUpdateMessageDto userUpdateMessageDto);

        /// <summary>
        /// The TriggerPodAccessDefinitionGenerationAsync
        /// </summary>
        /// <param name="roleUpdateMessageDto">The roleUpdateMessageDto<see cref="RoleUpdateMessageDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task TriggerPodAccessDefinitionGenerationAsync(RoleUpdateMessageDto roleUpdateMessageDto);

        /// <summary>
        /// The QueueBulkExportMessageAsync
        /// </summary>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task QueueBulkExportMessageAsync(string groupId);

        /// <summary>
        /// The QueueBulkImportMessageAsync
        /// </summary>
        /// <param name="bulkDataOperationID">The bulkDataOperationID<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task QueueBulkImportMessageAsync(string bulkDataOperationID);

        /// <summary>
        /// The TriggerReportExportAsync
        /// </summary>
        /// <param name="masterReportType">The masterReportType<see cref="string"/></param>
        /// <param name="reportType">The reportType<see cref="string"/></param>
        /// <param name="reportCriteria">The reportCriteria<see cref="object"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task TriggerReportExportAsync(string masterReportType, string reportType, object reportCriteria);
    }
}
