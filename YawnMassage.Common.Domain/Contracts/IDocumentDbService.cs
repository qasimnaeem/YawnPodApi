using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;

namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ISystemDocumentDbService" />
    /// </summary>
    public interface ISystemDocumentDbService : IDocumentDbService
    {
    }

    /// <summary>
    /// Defines the <see cref="ITenantDocumentDbService" />
    /// </summary>
    public interface ITenantDocumentDbService : IDocumentDbService
    {
    }

    /// <summary>
    /// Defines the <see cref="IDocumentDbService" />
    /// </summary>
    public interface IDocumentDbService
    {
        /// <summary>
        /// The CreateDocumentAsync
        /// </summary>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="document">The document<see cref="object"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateDocumentAsync(string collectionId, string partitionKey, object document);

        /// <summary>
        /// The ReplaceDocumentAsync
        /// </summary>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <param name="document">The document<see cref="object"/></param>
        /// <param name="eTag">The eTag<see cref="string"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> ReplaceDocumentAsync(string collectionId, string partitionKey, string documentId, object document, string eTag);

        /// <summary>
        /// The DeleteDocumentAsync
        /// </summary>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteDocumentAsync(string collectionId, string partitionKey, string documentId);

        /// <summary>
        /// The GetDocumentAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> GetDocumentAsync<T>(string collectionId, string partitionKey, string documentId);

        /// <summary>
        /// The DocumentExistsAsync
        /// </summary>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> DocumentExistsAsync(string collectionId, string partitionKey, string documentId);

        /// <summary>
        /// The GetDocumentsAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <returns>The <see cref="Task{List{TResult}}"/></returns>
        Task<List<TResult>> GetDocumentsAsync<T, TResult>(
            string collectionId, string partitionKey,
            Func<IQueryable<T>, IQueryable<TResult>> query);

        /// <summary>
        /// The GetDocumentsWithPagingAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="criteria">The criteria<see cref="ResultSetCriteria"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{TResult}}"/></returns>
        Task<PagedQueryResultSet<TResult>> GetDocumentsWithPagingAsync<T, TResult>(
            string collectionId, string partitionKey,
            Func<IQueryable<T>, IQueryable<TResult>> query,
            ResultSetCriteria criteria = null, Dictionary<string, object> arrayContainsReplacements = null);

        /// <summary>
        /// The ExecuteStoredProcedureAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collectionId">The collectionId<see cref="string"/></param>
        /// <param name="sprocId">The sprocId<see cref="string"/></param>
        /// <param name="sprocParams">The sprocParams<see cref="object[]"/></param>
        /// <returns>The <see cref="Task{TValue}"/></returns>
        Task<TValue> ExecuteStoredProcedureAsync<TValue>(string collectionId, string sprocId, params object[] sprocParams);
    }
}
