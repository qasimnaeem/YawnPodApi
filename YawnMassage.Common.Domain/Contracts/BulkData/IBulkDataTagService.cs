using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Documents;
namespace YawnMassage.Common.Domain.Contracts.BulkData
{

    //Marker interface with generic type constraint.
    /// <summary>
    /// Defines the <see cref="IBulkDataTagService{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBulkDataTagService<T> : IBulkDataTagService where T : AuditedDocumentBase, new()
    {
    }

    /// <summary>
    /// Defines the <see cref="IBulkDataTagService" />
    /// </summary>
    public interface IBulkDataTagService
    {
        /// <summary>
        /// The LoadTagsFromDatabaseAsync
        /// </summary>
        /// <param name="permittedGroupIds">The permittedGroupIds<see cref="IEnumerable{string}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task LoadTagsFromDatabaseAsync(IEnumerable<string> permittedGroupIds);

        /// <summary>
        /// The DefineTag
        /// </summary>
        /// <param name="tag">The tag<see cref="string"/></param>
        /// <param name="Id">The Id<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        void DefineTag(string tag, string Id, string groupId = "*");

        /// <summary>
        /// The GetIdForTag
        /// </summary>
        /// <param name="tag">The tag<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        string GetIdForTag(string tag, string groupId = "*");

        /// <summary>
        /// The GetTagForId
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        string GetTagForId(string id);

        /// <summary>
        /// The TagExists
        /// </summary>
        /// <param name="tag">The tag<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        bool TagExists(string tag, string groupId = "*");
    }
}
