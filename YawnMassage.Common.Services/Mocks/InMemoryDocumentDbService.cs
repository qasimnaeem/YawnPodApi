using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Services.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services.Mocks
{
    public class InMemoryDocumentDbService : IDocumentDbService
    {
        private const string Id = "id";
        private const string ETag = "_etag";

        /// <summary>
        /// Keeps unique documentIds per collection.
        /// </summary>
        Dictionary<string, HashSet<string>> documentIds = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// Keeps collections, partitions and documents inside partitions.
        /// </summary>
        Dictionary<string, Dictionary<string, Dictionary<string, JObject>>> collections = new Dictionary<string, Dictionary<string, Dictionary<string, JObject>>>();

        public Task<DocumentUpdateResultDto> CreateDocumentAsync(string collectionId, string partitionKey, object document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            var partition = GetPartition(collectionId, partitionKey);

            //Convert the object into JObject so we can inject json properties into it.
            var obj = JObject.FromObject(document);

            //Get the id specified in the document object (if any).
            string id = null;
            if (obj[Id] != null)
            {
                id = obj[Id].ToObject<string>();

                if (string.IsNullOrWhiteSpace(id))
                    id = null;
                //Check whether the documentId already exists.
                else if (GetDocumentIdSetForCollection(collectionId).Contains(id))
                    throw new Exception("Unique key violation.");
            }

            //If the document does not have an id specified, generate an id.
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
                obj[Id] = id;
            }

            //Generate the etag.
            string etag = Guid.NewGuid().ToString("N");
            obj[ETag] = etag;

            partition[id] = obj;
            GetDocumentIdSetForCollection(collectionId).Add(id);

            return Task.FromResult(new DocumentUpdateResultDto
            {
                Id = id,
                ETag = etag
            });
        }

        public Task<DocumentUpdateResultDto> ReplaceDocumentAsync(string collectionId, string partitionKey, string documentId, object document, string eTag)
        {
            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentNullException(nameof(documentId));

            if (document == null)
                throw new ArgumentNullException(nameof(document));

            var partition = GetPartition(collectionId, partitionKey);
            var obj = partition[documentId];

            if (eTag != null && !eTag.Equals(obj["_etag"].ToString()))
                throw new Exception("Concurrency violation.");

            var newObj = JObject.FromObject(document);
            string etag = Guid.NewGuid().ToString("N");
            newObj[ETag] = etag;

            partition[documentId] = newObj;

            return Task.FromResult(new DocumentUpdateResultDto
            {
                Id = documentId,
                ETag = etag
            });
        }

        public Task DeleteDocumentAsync(string collectionId, string partitionKey, string documentId)
        {
            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentNullException(nameof(documentId));

            var partition = GetPartition(collectionId, partitionKey);
            partition.Remove(documentId);
            GetDocumentIdSetForCollection(collectionId).Remove(documentId);

            return Task.CompletedTask;
        }

        public Task<T> GetDocumentAsync<T>(string collectionId, string partitionKey, string documentId)
        {
            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentNullException(nameof(documentId));

            var partition = GetPartition(collectionId, partitionKey);
            if (partition.ContainsKey(documentId))
                return Task.FromResult(partition[documentId].ToObject<T>());

            return Task.FromResult(default(T));
        }

        public Task<bool> DocumentExistsAsync(string collectionId, string partitionKey, string documentId)
        {
            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentNullException(nameof(documentId));

            var partition = GetPartition(collectionId, partitionKey);
            return Task.FromResult(partition.ContainsKey(documentId));
        }

        public async Task<List<TResult>> GetDocumentsAsync<T, TResult>(string collectionId, string partitionKey, Func<IQueryable<T>, IQueryable<TResult>> query)
        {
            var resultSet = await GetDocumentsWithPagingAsync(collectionId, partitionKey, query, null);
            return resultSet.Results;
        }

        public Task<PagedQueryResultSet<TResult>> GetDocumentsWithPagingAsync<T, TResult>(string collectionId, string partitionKey,
            Func<IQueryable<T>, IQueryable<TResult>> query, ResultSetCriteria criteria = null,
            Dictionary<string, object> arrayContainsReplacements = null)
        {
            int pageSize = criteria?.Limit ?? 0;
            string continuationToken = criteria?.PageToken;
            var partition = GetPartition(collectionId, partitionKey);

            //In-memory provider needs to filter out the matching typed objects.
            var allObjectsOfType = GetObjectsOfType<T>(partition.Values.ToList());

            var skip = (continuationToken == null ? 0 : int.Parse(continuationToken));

            var objQuery = allObjectsOfType.Select(obj => obj.ToObject<T>()).AsQueryable();
            if (criteria != null)
                objQuery = objQuery.OrderByFieldName(criteria.SortBy, criteria.IsAscending());

            var results = query(objQuery).Skip(skip);
            var nextSkip = 0;

            if (pageSize > 0)
            {
                var fullCount = results.Count();
                if (fullCount > pageSize)
                    nextSkip = skip + pageSize;

                results = results.Take(pageSize);
            }

            return Task.FromResult(new PagedQueryResultSet<TResult>
            {
                Results = results.ToList(),
                ContinuationToken = nextSkip.ToString()
            });
        }

        public Task<TValue> ExecuteStoredProcedureAsync<TValue>(string collectionId, string sprocId, params object[] sprocParams)
        {
            throw new NotImplementedException();
        }

        #region Private helper methods

        private List<JObject> GetObjectsOfType<T>(IEnumerable<JObject> list)
        {
            var typedObject = Activator.CreateInstance<T>();

            var typeProperty = typeof(T).GetProperty("DocType");
            var typeString = (string)typeProperty?.GetValue(typedObject);

            if (!string.IsNullOrEmpty(typeString))
            {
                return list.Where(obj => obj["docType"].Value<string>() == typeString).ToList();
            }
            else
            {
                return list.ToList();
            }
        }

        private HashSet<string> GetDocumentIdSetForCollection(string collectionId)
        {
            if (string.IsNullOrEmpty(collectionId))
                throw new ArgumentNullException(nameof(collectionId));

            if (!documentIds.ContainsKey(collectionId))
                documentIds[collectionId] = new HashSet<string>();

            return documentIds[collectionId];
        }

        private Dictionary<string, Dictionary<string, JObject>> GetCollection(string collectionId)
        {
            if (string.IsNullOrEmpty(collectionId))
                throw new ArgumentNullException(nameof(collectionId));

            if (!collections.ContainsKey(collectionId))
                collections[collectionId] = new Dictionary<string, Dictionary<string, JObject>>();

            return collections[collectionId];
        }

        private Dictionary<string, JObject> GetPartition(string collectionId, string partitionKey)
        {
            if (string.IsNullOrEmpty(partitionKey))
                throw new ArgumentNullException(nameof(partitionKey));

            var collection = GetCollection(collectionId);

            if (!collection.ContainsKey(partitionKey))
                collection[partitionKey] = new Dictionary<string, JObject>();

            return collection[partitionKey];
        }

        #endregion
    }
}
