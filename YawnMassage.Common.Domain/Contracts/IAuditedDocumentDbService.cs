using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;

namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IAuditedDocumentDbService" />
    /// </summary>
    public interface IAuditedDocumentDbService
    {
        /// <summary>
        /// The CreateDocumentAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document<see cref="T"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateDocumentAsync<T>(T document) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The CreateDocumentAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document<see cref="T"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateDocumentAsync<T>(T document, string partitionKey) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The ReplaceDocumentAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document<see cref="T"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> ReplaceDocumentAsync<T>(T document) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The DeleteDocumentAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteDocumentAsync<T>(T document) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The DeleteDocumentByIdAsync
        /// </summary>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteDocumentByIdAsync(string documentId);

        /// <summary>
        /// The GetDocumentAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> GetDocumentAsync<T>(string documentId) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The DocumentExistsAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> DocumentExistsAsync<T>(string documentId) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The SoftDeleteDocumentsAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentIds">The documentIds<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SoftDeleteDocumentsAsync<T>(params string[] documentIds) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The FirstOrDefaultAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="includeDeleted">The includeDeleted<see cref="bool"/></param>
        /// <returns>The <see cref="Task{TResult}"/></returns>
        Task<TResult> FirstOrDefaultAsync<T, TResult>(Func<IQueryable<T>, IQueryable<TResult>> query, bool includeDeleted = false) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The GetDocumentsAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="includeDeleted">The includeDeleted<see cref="bool"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{List{TResult}}"/></returns>
        Task<List<TResult>> GetDocumentsAsync<T, TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> query,
            bool includeDeleted = false, Dictionary<string, object> arrayContainsReplacements = null) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The GetDocumentsAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="includeDeleted">The includeDeleted<see cref="bool"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{List{TResult}}"/></returns>
        Task<List<TResult>> GetDocumentsAsync<T, TResult>(
          Func<IQueryable<T>, IQueryable<TResult>> query,
           string partitionKey, bool includeDeleted = false, Dictionary<string, object> arrayContainsReplacements = null) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The AnyAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{T}}"/></param>
        /// <param name="includeDeleted">The includeDeleted<see cref="bool"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> AnyAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, bool includeDeleted = false) where T : AuditedDocumentBase, new();

        /// <summary>
        /// Get documents with paging (continuation token)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="criteria">The criteria<see cref="ResultSetCriteria"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{TResult}}"/></returns>
        Task<PagedQueryResultSet<TResult>> GetDocumentsWithPagingAsync<T, TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> query,
            ResultSetCriteria criteria, Dictionary<string, object> arrayContainsReplacements = null) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The GetDocumentsWithPagingAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="criteria">The criteria<see cref="ResultSetCriteria"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{TResult}}"/></returns>
        Task<PagedQueryResultSet<TResult>> GetDocumentsWithPagingAsync<T, TResult>(
           Func<IQueryable<T>, IQueryable<TResult>> query,
           ResultSetCriteria criteria, string partitionKey, Dictionary<string, object> arrayContainsReplacements = null) where T : AuditedDocumentBase, new();

        /// <summary>
        /// The GetStoredProcedureValueAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sprocId">The sprocId<see cref="string"/></param>
        /// <param name="sprocParams">The sprocParams<see cref="object[]"/></param>
        /// <returns>The <see cref="Task{TValue}"/></returns>
        Task<TValue> GetStoredProcedureValueAsync<TValue>(string sprocId, params object[] sprocParams);

        /// <summary>
        /// The ExecuteStoredProcedureAsync
        /// </summary>
        /// <param name="sprocId">The sprocId<see cref="string"/></param>
        /// <param name="sprocParams">The sprocParams<see cref="object[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ExecuteStoredProcedureAsync(string sprocId, params object[] sprocParams);
    }
}
