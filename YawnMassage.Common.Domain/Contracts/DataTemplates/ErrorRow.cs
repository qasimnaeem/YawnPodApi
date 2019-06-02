using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Defines the <see cref="ErrorRow" />
    /// </summary>
    public class ErrorRow
    {
        /// <summary>
        /// Gets or sets the RowIndex
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the ErrorCells
        /// </summary>
        public IEnumerable<IDataCell> ErrorCells { get; set; }
    }
}
