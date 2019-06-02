using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Represents a spreadsheet compatible data store.
    /// </summary>
    public interface ITemplateSheetDataStore
    {
        /// <summary>
        /// Gets the ColumnHeadersRowIndex
        /// </summary>
        int ColumnHeadersRowIndex { get; }

        /// <summary>
        /// Gets the SheetName
        /// </summary>
        string SheetName { get; }

        /// <summary>
        /// The GetCellValue
        /// </summary>
        /// <param name="rowIndex">The rowIndex<see cref="int"/></param>
        /// <param name="cellIndex">The cellIndex<see cref="int"/></param>
        /// <returns>The <see cref="object"/></returns>
        object GetCellValue(int rowIndex, int cellIndex);

        /// <summary>
        /// The SetCellValue
        /// </summary>
        /// <param name="rowIndex">The rowIndex<see cref="int"/></param>
        /// <param name="cellIndex">The cellIndex<see cref="int"/></param>
        /// <param name="value">The value<see cref="object"/></param>
        void SetCellValue(int rowIndex, int cellIndex, object value);

        /// <summary>
        /// The ReadColumnNames
        /// </summary>
        /// <returns>The <see cref="List{string}"/></returns>
        List<string> ReadColumnNames();

        /// <summary>
        /// The WriteColumnNames
        /// </summary>
        /// <param name="columnNames">The columnNames<see cref="IEnumerable{string}"/></param>
        void WriteColumnNames(IEnumerable<string> columnNames);

        /// <summary>
        /// The ReadDataRows
        /// </summary>
        /// <returns>The <see cref="List{IDataRow}"/></returns>
        List<IDataRow> ReadDataRows();

        /// <summary>
        /// The WriteDataRows
        /// </summary>
        /// <param name="datarows">The datarows<see cref="List{Dictionary{string, object}}"/></param>
        void WriteDataRows(List<Dictionary<string, object>> datarows);

        /// <summary>
        /// The ApplyStyle
        /// </summary>
        /// <param name="styleReference">The styleReference<see cref="object"/></param>
        /// <param name="rowIndex">The rowIndex<see cref="int"/></param>
        /// <param name="cellIndex">The cellIndex<see cref="int?"/></param>
        void ApplyStyle(object styleReference, int rowIndex, int? cellIndex = null);
    }
}
