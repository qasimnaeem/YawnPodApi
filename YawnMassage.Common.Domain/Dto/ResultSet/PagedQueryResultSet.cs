using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Dto.ResultSet
{
    /// <summary>
    /// Defines the <see cref="PagedQueryResultSet{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedQueryResultSet<T>
    {
        /// <summary>
        /// Gets or sets the Results
        /// </summary>
        public List<T> Results { get; set; }

        /// <summary>
        /// Gets or sets the ContinuationToken
        /// </summary>
        public string ContinuationToken { get; set; }
    }
}
