using YawnMassage.Common.Domain.Contracts.DataTemplates;

namespace YawnMassage.Common.Services.DataTemplates
{
    /// <summary>
    /// Represents a cell in the template which is capable of holding error conditions
    /// and additional information.
    /// </summary>
    public class DataCell : IDataCell
    {
        public object Value { get; }
        public string ColumnName { get; }
        public bool HasRequiredError { get; private set; }
        public bool HasDataFormatError { get; private set; }
        public int RowIndex { get; }
        public int ColIndex { get; }

        public DataCell(string columnName, object value, int rowIndex, int colIndex)
        {
            Value = value;
            ColumnName = columnName;
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }

        public void MarkAsRequired()
        {
            HasRequiredError = true;
        }

        public void MarkAsInvalidDataFormat()
        {
            HasDataFormatError = true;
        }
    }
}
