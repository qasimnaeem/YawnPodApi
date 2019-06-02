using System;
namespace YawnMassage.Common.Domain.Dto.ResultSet
{
    /// <summary>
    /// Defines the <see cref="ResultSetCriteria" />
    /// </summary>
    public class ResultSetCriteria
    {
        /// <summary>
        /// Defines the Descending
        /// </summary>
        private const string Descending = "desc";

        /// <summary>
        /// Gets or sets the Limit
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the PageToken
        /// </summary>
        public string PageToken { get; set; }

        /// <summary>
        /// Gets or sets the SortBy
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets the SortDir
        /// </summary>
        public string SortDir { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IncludeDeleted
        /// </summary>
        public bool IncludeDeleted { get; set; }

        /// <summary>
        /// The IsAscending
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        public bool IsAscending()
        {
            return SortDir == null || !SortDir.Equals(Descending, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
