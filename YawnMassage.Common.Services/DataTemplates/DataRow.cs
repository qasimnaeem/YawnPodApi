using YawnMassage.Common.Domain.Contracts.DataTemplates;
using System.Collections.Generic;

namespace YawnMassage.Common.Services.DataTemplates
{
    /// <summary>
    /// Represents a row in the template which is capable of holding error conditions
    /// and additional information.
    /// </summary>
    public class DataRow : IDataRow
    {
        private Dictionary<string, IDataCell> _cellDictionary = new Dictionary<string, IDataCell>();
        private List<IDataCell> _cells = new List<IDataCell>();

        public IEnumerable<IDataCell> Cells => _cells;
        public bool IsIndented { get; set; }
        public bool HasError { get; private set; }
        public int RowIndex { get; private set; }
        public string ErrorMessage { get; private set; }

        public DataRow(int rowIndex)
        {
            RowIndex = rowIndex;
        }

        public IDataCell GetCellByColumnName(string columnName)
        {
            return _cellDictionary[columnName.ToLower()];
        }
        
        public void AddCell(IDataCell cell, string columnName)
        {
            _cells.Add(cell);
            _cellDictionary.Add(columnName.ToLower(), cell);
        }

        public void MarkAsError(string message = null)
        {
            HasError = true;
            ErrorMessage = message;
        }
    }
}
