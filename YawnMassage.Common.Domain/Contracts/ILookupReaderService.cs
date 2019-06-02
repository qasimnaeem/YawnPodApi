using System.Threading.Tasks;
using YawnMassage.Common.Domain.Documents;

namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILookupReaderService" />
    /// </summary>
    public interface ILookupReaderService
    {
        /// <summary>
        /// The GetLookupAsync
        /// </summary>
        /// <param name="lookupKey">The lookupKey<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <param name="culture">The culture<see cref="string"/></param>
        /// <param name="section">The section<see cref="string"/></param>
        /// <returns>The <see cref="Task{Lookup}"/></returns>
        Task<Lookup> GetLookupAsync(string lookupKey, string groupId = null, string culture = null, string section = null);

        /// <summary>
        /// The GetTextAsync
        /// </summary>
        /// <param name="lookupKey">The lookupKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <param name="culture">The culture<see cref="string"/></param>
        /// <param name="section">The section<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetTextAsync(string lookupKey, string value, string groupId = null, string culture = null, string section = null);
    }
}
