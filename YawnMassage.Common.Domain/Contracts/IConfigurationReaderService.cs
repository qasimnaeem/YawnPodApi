using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IConfigurationReaderService" />
    /// </summary>
    public interface IConfigurationReaderService
    {
        /// <summary>
        /// The GetValueAsync
        /// </summary>
        /// <param name="configurationKey">The configurationKey<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <param name="culture">The culture<see cref="string"/></param>
        /// <param name="section">The section<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetValueAsync(string configurationKey, string groupId = null, string culture = null, string section = null);
    }
}
