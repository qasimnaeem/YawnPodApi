namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Represents a cell in the template which is capable of holding error conditions
    /// and additional information.
    /// </summary>
    public interface IDataCell
    {
        /// <summary>
        /// Gets the ColumnName
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Gets the Value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets a value indicating whether HasRequiredError
        /// </summary>
        bool HasRequiredError { get; }

        /// <summary>
        /// Gets a value indicating whether HasDataFormatError
        /// </summary>
        bool HasDataFormatError { get; }

        /// <summary>
        /// Gets the RowIndex
        /// </summary>
        int RowIndex { get; }

        /// <summary>
        /// Gets the ColIndex
        /// </summary>
        int ColIndex { get; }

        /// <summary>
        /// The MarkAsRequired
        /// </summary>
        void MarkAsRequired();

        /// <summary>
        /// The MarkAsInvalidDataFormat
        /// </summary>
        void MarkAsInvalidDataFormat();
    }
}
