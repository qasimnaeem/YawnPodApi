using System.Collections.Generic;
using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IAlertContextObjectService" />
    /// </summary>
    public interface IAlertContextObjectService
    {
        /// <summary>
        /// The GetContextObjectKeyValuesAsync
        /// </summary>
        /// <param name="objectInfo">The objectInfo<see cref="object"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <param name="culture">The culture<see cref="string"/></param>
        /// <param name="timeZone">The timeZone<see cref="string"/></param>
        /// <returns>The <see cref="Task{Dictionary{string, string}}"/></returns>
        Task<Dictionary<string, string>> GetContextObjectKeyValuesAsync(object objectInfo, string groupId, string culture, string timeZone);
    }
}
