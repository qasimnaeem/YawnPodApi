using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto.ResultSet;

namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IEventsDbService" />
    /// </summary>
    public interface IEventsDbService
    {
        /// <summary>
        /// The AnyAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{T}}"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> AnyAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, string partitionKey) where T : EventDocumentBase, new();

        /// <summary>
        /// The ExecuteStoredProcedureAsync
        /// </summary>
        /// <param name="sprocId">The sprocId<see cref="string"/></param>
        /// <param name="sprocParams">The sprocParams<see cref="object[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ExecuteStoredProcedureAsync(string sprocId, params object[] sprocParams);

        /// <summary>
        /// The FirstOrDefaultAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <returns>The <see cref="Task{TResult}"/></returns>
        Task<TResult> FirstOrDefaultAsync<T, TResult>(Func<IQueryable<T>, IQueryable<TResult>> query, string partitionKey) where T : EventDocumentBase, new();

        /// <summary>
        /// The GetEventAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentId">The documentId<see cref="string"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> GetEventAsync<T>(string documentId, string partitionKey) where T : EventDocumentBase, new();

        /// <summary>
        /// The GetEventsAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{List{TResult}}"/></returns>
        Task<List<TResult>> GetEventsAsync<T, TResult>(Func<IQueryable<T>, IQueryable<TResult>> query, string partitionKey, Dictionary<string, object> arrayContainsReplacements = null) where T : EventDocumentBase, new();

        /// <summary>
        /// The GetEventsWithPagingAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">The query<see cref="Func{IQueryable{T}, IQueryable{TResult}}"/></param>
        /// <param name="criteria">The criteria<see cref="ResultSetCriteria"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="arrayContainsReplacements">The arrayContainsReplacements<see cref="Dictionary{string, object}"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{TResult}}"/></returns>
        Task<PagedQueryResultSet<TResult>> GetEventsWithPagingAsync<T, TResult>(Func<IQueryable<T>, IQueryable<TResult>> query, ResultSetCriteria criteria, string partitionKey, Dictionary<string, object> arrayContainsReplacements = null) where T : EventDocumentBase, new();

        /// <summary>
        /// The GetStoredProcedureValueAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sprocId">The sprocId<see cref="string"/></param>
        /// <param name="sprocParams">The sprocParams<see cref="object[]"/></param>
        /// <returns>The <see cref="Task{TValue}"/></returns>
        Task<TValue> GetStoredProcedureValueAsync<TValue>(string sprocId, params object[] sprocParams);

        /// <summary>
        /// The InsertEventAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document">The document<see cref="T"/></param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task InsertEventAsync<T>(T document, string partitionKey) where T : EventDocumentBase, new();
    }
}
