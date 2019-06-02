using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts.BulkData;
using YawnMassage.Common.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services.BulkData
{
    /// <summary>
    /// Provides common functionality for all bulk data tag services. All object-specific tag services
    /// should inherit from this class.
    /// </summary>
    public abstract class BulkDataTagServiceBase<T> : IBulkDataTagService<T> where T : AuditedDocumentBase, new()
    {
        /// <summary>
        /// Dictionary with Tag-to-Id mappings.
        /// </summary>
        private readonly Dictionary<string, string> _tagDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary with Id-to-Tag mappings.
        /// </summary>
        private readonly Dictionary<string, string> _idDictionary = new Dictionary<string, string>();

        public abstract Task LoadTagsFromDatabaseAsync(IEnumerable<string> permittedGroupIds);

        public string GetTagForId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var tag = _idDictionary[id];
            return tag;
        }

        public string GetIdForTag(string tag, string groupId = "*")
        {
            if (string.IsNullOrEmpty(tag))
                return null;

            var fullTagName = GetFullTagName(tag, groupId);
            var id = _tagDictionary[fullTagName];
            return id;
        }

        public void DefineTag(string tag, string Id, string groupId = "*")
        {
            //Check if tag already exists.
            if (TagExists(tag, groupId))
                throw new Exception($"Tag '{tag}' already exists.");

            var fullTagName = GetFullTagName(tag, groupId);
            _tagDictionary[fullTagName] = Id;
            _idDictionary[Id] = tag;
        }

        public bool TagExists(string tag, string groupId = "*")
        {
            var fullTagName = GetFullTagName(tag, groupId);
            var tagExists = _tagDictionary.ContainsKey(fullTagName);
            return tagExists;
        }

        private string GetFullTagName(string tag, string groupId)
        {
            return $"{groupId}-{tag}";
        }
    }
}
