using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    /// <summary>
    /// This service introduces automatic partition management, audit fields and soft-delete concepts
    /// on top of plain document db service.
    /// </summary>
    public class EventsDbService : IEventsDbService
    {
        private readonly IDocumentDbService _documentDbService;
        public readonly RequestContext _requestContext;
        private readonly string _collectionId;

        public EventsDbService(IDocumentDbService documentDbService, DbInfo dbInfo, RequestContext requestContext)
        {
            _documentDbService = documentDbService;
            _requestContext = requestContext;
            _collectionId = dbInfo.EventsCollectionId;
        }
        
        public virtual async Task InsertEventAsync<T>(T document, string partitionKey) where T : EventDocumentBase, new()
        {
            if (!string.IsNullOrEmpty(document.GroupId) && document.GroupId != GetGroupId())
                throw new UnauthorizedAccessException("GroupId context mismatch.");

            document.PartitionKey = partitionKey;
            document.GroupId = GetGroupId();

            var result = await _documentDbService.CreateDocumentAsync(_collectionId, document.PartitionKey, document);

            //Update the in-memory object with the id generated from the db.
            document.Id = result.Id;
        }

        public virtual async Task<T> GetEventAsync<T>(string documentId, string partitionKey) where T : EventDocumentBase, new()
        {
            var document = await _documentDbService.GetDocumentAsync<T>(_collectionId, partitionKey, documentId);

            if (document == null)
                throw new DocumentNotFoundException();

            return document;
        }

        public virtual async Task<TResult> FirstOrDefaultAsync<T, TResult>(Func<IQueryable<T>, IQueryable<TResult>> query, string partitionKey) where T : EventDocumentBase, new()
        {
            var criteria = new ResultSetCriteria { Limit = 1 };
            var resultSet = await GetEventsWithPagingAsync(query, criteria, partitionKey);
            return resultSet.Results.FirstOrDefault();
        }

        public virtual async Task<List<TResult>> GetEventsAsync<T, TResult>(Func<IQueryable<T>, IQueryable<TResult>> query,
            string partitionKey, Dictionary<string, object> arrayContainsReplacements = null) where T : EventDocumentBase, new()
        {
            var criteria = new ResultSetCriteria();
            var resultSet = await GetEventsWithPagingAsync(query, criteria, partitionKey, arrayContainsReplacements);
            return resultSet.Results;
        }

        public virtual async Task<bool> AnyAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, string partitionKey) where T : EventDocumentBase, new()
        {
            var criteria = new ResultSetCriteria { Limit = 1 };
            var resultSet = await GetEventsWithPagingAsync<T, string>(q => query(q).Select(d => d.Id), criteria, partitionKey);
            return resultSet.Results.Count > 0;
        }

        public virtual async Task<PagedQueryResultSet<TResult>> GetEventsWithPagingAsync<T, TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> query,
            ResultSetCriteria criteria, string partitionKey, Dictionary<string, object> arrayContainsReplacements = null) where T : EventDocumentBase, new()
        {
            var type = (new T()).DocType;
            var resultSet = await _documentDbService.GetDocumentsWithPagingAsync<T, TResult>(_collectionId, partitionKey,
                q => query(q.Where(doc => doc.DocType == type)),
                criteria, arrayContainsReplacements);

            return resultSet;
        }

        public virtual async Task<TValue> GetStoredProcedureValueAsync<TValue>(string sprocId, params object[] sprocParams)
        {
            return await _documentDbService.ExecuteStoredProcedureAsync<TValue>(_collectionId, sprocId, sprocParams);
        }

        public virtual async Task ExecuteStoredProcedureAsync(string sprocId, params object[] sprocParams)
        {
            await _documentDbService.ExecuteStoredProcedureAsync<object>(_collectionId, sprocId, sprocParams);
        }

        #region Private helper methods
        
        private string GetGroupId()
        {
            return _requestContext.GroupId;
        }

        #endregion
    }
}
