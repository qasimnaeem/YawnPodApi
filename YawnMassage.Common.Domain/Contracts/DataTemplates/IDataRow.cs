using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{   
    /// <summary>
    /// Represents a row in the template which is capable of holding error conditions
    /// and additional information.
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// Gets a value indicating whether IsIndented
        /// </summary>
        bool IsIndented { get; }

        /// <summary>
        /// Gets a value indicating whether HasError
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// Gets the ErrorMessage
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets the RowIndex
        /// </summary>
        int RowIndex { get; }

        /// <summary>
        /// Gets the Cells
        /// </summary>
        IEnumerable<IDataCell> Cells { get; }

        /// <summary>
        /// The GetCellByColumnName
        /// </summary>
        /// <param name="columnName">The columnName<see cref="string"/></param>
        /// <returns>The <see cref="IDataCell"/></returns>
        IDataCell GetCellByColumnName(string columnName);

        /// <summary>
        /// The MarkAsError
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        void MarkAsError(string message = null);
    }
}
