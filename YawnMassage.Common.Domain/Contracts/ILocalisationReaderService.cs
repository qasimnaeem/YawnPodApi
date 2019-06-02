using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILocalisationReaderService" />
    /// </summary>
    public interface ILocalisationReaderService
    {
        /// <summary>
        /// The GetTextAsync
        /// </summary>
        /// <param name="localisationKey">The localisationKey<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <param name="culture">The culture<see cref="string"/></param>
        /// <param name="section">The section<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetTextAsync(string localisationKey, string groupId = null, string culture = null, string section = null);
    }
}
