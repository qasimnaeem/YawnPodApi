using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YawnMassage.Common.Services.Extensions;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Exceptions;
using YawnMassage.Common.Domain.Dto.ResultSet;

namespace YawnMassage.Common.Services
{
    /// <summary>
    /// A wrapper class for Microsoft DocumentClient.
    public class CosmosDbService : IDocumentDbService, IDisposable
    {
        private readonly string _databaseId;
        private readonly bool _disableConcurrencyCheck;
        private DocumentClient _client;

        private const string NotFound = "NotFound";

        public CosmosDbService(DbInfo dbInfo)
        {
            _databaseId = dbInfo.DatabaseId;
            _disableConcurrencyCheck = dbInfo.DisableConcurrencyCheck;

            _client = new DocumentClient(new Uri(dbInfo.ServiceEndpoint), dbInfo.SasKey);
        }

        public virtual async Task<DocumentUpdateResultDto> CreateDocumentAsync(string collectionId, string partitionKey, object document)
        {
            var resp = await _client.CreateDocumentAsync(CreateCollectionUri(collectionId), document, CreateRequestOptions(partitionKey));
            return new DocumentUpdateResultDto
            {
                Id = resp.Resource.Id,
                ETag = resp.Resource.ETag
            };
        }

        public virtual async Task<DocumentUpdateResultDto> ReplaceDocumentAsync(string collectionId, string partitionKey, string documentId, object document, string eTag)
        {
            try
            {
                var resp = await _client.ReplaceDocumentAsync(CreateDocumentUri(collectionId, documentId), document, CreateRequestOptionsWithETag(partitionKey, eTag));
                return new DocumentUpdateResultDto
                {
                    Id = resp.Resource.Id,
                    ETag = resp.Resource.ETag
                };
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                    throw new ConcurrencyException();

                throw;
            }
        }

        public virtual async Task DeleteDocumentAsync(string collectionId, string partitionKey, string documentId)
        {
            await _client.DeleteDocumentAsync(CreateDocumentUri(collectionId, documentId), CreateRequestOptions(partitionKey));
        }

        public virtual async Task<T> GetDocumentAsync<T>(string collectionId, string partitionKey, string documentId)
        {
            try
            {
                var resp = await _client.ReadDocumentAsync<T>(CreateDocumentUri(collectionId, documentId), CreateRequestOptions(partitionKey));
                return resp.Document;
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == NotFound)
                    return default(T);
                else
                    throw;
            }
        }

        public virtual async Task<bool> DocumentExistsAsync(string collectionId, string partitionKey, string documentId)
        {
            var q = _client.CreateDocumentQuery(
                CreateCollectionUri(collectionId),
                new FeedOptions
                {
                    PartitionKey = new PartitionKey(partitionKey),
                    MaxItemCount = 1
                })
                .Where(d => d.Id == documentId)
                .Select(d => d.Id).AsDocumentQuery();

            var results = await q.ExecuteNextAsync();

            return results.Count > 0;
        }

        public virtual async Task<List<TResult>> GetDocumentsAsync<T, TResult>(
            string collectionId, string partitionKey,
            Func<IQueryable<T>, IQueryable<TResult>> query)
        {
            var resultSet = await GetDocumentsWithPagingAsync(collectionId, partitionKey, query);
            return resultSet.Results;
        }

        public virtual async Task<PagedQueryResultSet<TResult>> GetDocumentsWithPagingAsync<T, TResult>(
            string collectionId, string partitionKey,
            Func<IQueryable<T>, IQueryable<TResult>> query,
            ResultSetCriteria criteria = null, Dictionary<string, object> arrayContainsReplacements = null)
        {
            int pageSize = criteria?.Limit ?? 0;
            string continuationToken = criteria?.PageToken;

            var collectionUri = CreateCollectionUri(collectionId);
            var feedOptions = new FeedOptions
            {
                PartitionKey = new PartitionKey(partitionKey),

                //Set MaxItemCount to a fixed value when pageSize = 0
                MaxItemCount = pageSize == 0 ? 500 : pageSize,
                RequestContinuation = continuationToken
            };

            var q = _client.CreateDocumentQuery<T>(collectionUri, feedOptions);

            var documentQuery = criteria != null ?
                query(q.OrderByFieldName(criteria.SortBy, criteria.IsAscending())).AsDocumentQuery() :
                query(q).AsDocumentQuery();

            //Use ARRAY_CONTAINS hack, if replacements are specified.
            if (arrayContainsReplacements != null)
            {
                var builder = new ArrayContainsExpressionBuilder<TResult>(documentQuery, arrayContainsReplacements);
                var updatedSql = builder.Build();
                documentQuery = _client.CreateDocumentQuery<TResult>(collectionUri, updatedSql, feedOptions).AsDocumentQuery();
            }

            var results = new List<TResult>();
            FeedResponse<TResult> response = null;

            do
            {
                response = await documentQuery.ExecuteNextAsync<TResult>();
                results.AddRange(response.ToList());

                //Stop the loop after first iteration when pageSize > 0 because we are getting one page at a time.
            } while (pageSize == 0 && documentQuery.HasMoreResults);

            return new PagedQueryResultSet<TResult>
            {
                Results = results,
                ContinuationToken = response.ResponseContinuation
            };
        }

        public virtual async Task<TValue> ExecuteStoredProcedureAsync<TValue>(string collectionId, string sprocId, params object[] sprocParams)
        {
            var sprocUri = CreateStoredProcedureUri(collectionId, sprocId);
            var resp = await _client.ExecuteStoredProcedureAsync<TValue>(sprocUri, sprocParams);
            return resp.Response;
        }

        #region Private helper methods

        private Uri CreateCollectionUri(string collectionId)
        {
            return UriFactory.CreateDocumentCollectionUri(_databaseId, collectionId);
        }

        private Uri CreateDocumentUri(string collectionId, string documentId)
        {
            return UriFactory.CreateDocumentUri(_databaseId, collectionId, documentId);
        }

        private Uri CreateStoredProcedureUri(string collectionId, string sprocId)
        {
            return UriFactory.CreateStoredProcedureUri(_databaseId, collectionId, sprocId);
        }

        private RequestOptions CreateRequestOptions(string partitionKey)
        {
            return new RequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }

        private RequestOptions CreateRequestOptionsWithETag(string partitionKey, string eTag)
        {
            var requestOptions = new RequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };

            if (!_disableConcurrencyCheck)
            {
                requestOptions.AccessCondition = new AccessCondition
                {
                    Condition = eTag,
                    Type = AccessConditionType.IfMatch
                };
            }

            return requestOptions;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                    _client = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CosmosDbStorageService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }


    /// <summary>
    /// A hack to inject ARRAY_CONTAINS partial match feature into the SQL expression generated
    /// by CosmosDB LINQ provider. Default LINQ Contains() only supports full match.
    /// </summary>
    public class ArrayContainsExpressionBuilder<T>
    {
        private readonly IDocumentQuery<T> _query;
        private readonly Dictionary<string, object> _replacements;

        const string ArrayContainsRegEx = @"ARRAY_CONTAINS\(.*?\[""(?<field>.*?)""\],(?<value>.*?)\)";

        public ArrayContainsExpressionBuilder(IDocumentQuery<T> query, Dictionary<string, object> replacements)
        {
            _query = query;
            _replacements = replacements;
        }

        public string Build()
        {
            var expressionObj = JObject.Parse(_query.ToString());
            var sql = expressionObj["query"].Value<string>();
            
            var regex = new Regex(ArrayContainsRegEx);
            return regex.Replace(sql, MatchEval);
        }

        private string MatchEval(Match match)
        {
            var fieldName = match.Groups["field"].Value;
            var replacement = JsonConvert.SerializeObject(_replacements[fieldName]) + ",true";

            var fieldValue = match.Groups["value"].Value;

            var str = match.ToString();
            return str.Replace(fieldValue, replacement);
        }
    }
}
