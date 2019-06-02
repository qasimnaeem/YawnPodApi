using YawnMassage.Common.Domain.Contracts.DataTemplates;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace YawnMassage.Common.Services.DataTemplates
{
    /// <summary>
    /// Wraps an Excel worksheet.
    /// </summary>
    public class TemplateSheetDataStore : ITemplateSheetDataStore
    {
        private readonly ISheet _sheet;
        private readonly ICellStyle _dateTimeCellStyle, _dateCellStyle;

        public int ColumnHeadersRowIndex { get; }

        public TemplateSheetDataStore(ISheet sheet, int columnHeadersRowNumber)
        {
            _sheet = sheet;
            ColumnHeadersRowIndex = columnHeadersRowNumber - 1;

            _dateTimeCellStyle = _sheet.Workbook.CreateCellStyle();
            _dateTimeCellStyle.DataFormat = _sheet.Workbook.CreateDataFormat().GetFormat("yyyy-MM-dd h:mm:ss AM/PM");

            _dateCellStyle = _sheet.Workbook.CreateCellStyle();
            _dateCellStyle.DataFormat = _sheet.Workbook.CreateDataFormat().GetFormat("yyyy-MM-dd");
        }

        public string SheetName => _sheet.SheetName;

        public List<string> ReadColumnNames()
        {
            var row = _sheet.GetRow(ColumnHeadersRowIndex);
            var names = new List<string>();

            var colIndex = 0;

            while (true)
            {
                var cell = row.GetCell(colIndex);
                if (IsCellEmpty(cell))
                    break;

                var value = GetCellValue(cell);
                if (value != null)
                {
                    if (value is string)
                    {
                        var str = (value as string).Trim();
                        if (str.Length > 0)
                            names.Add(str.ToLower());
                    }
                    else
                    {
                        names.Add(value.ToString());
                    }

                    colIndex++;
                }
            }

            return names;
        }

        public void WriteColumnNames(IEnumerable<string> columnNames)
        {
            var columnHeaderFont = _sheet.Workbook.CreateFont();
            columnHeaderFont.IsBold = true;

            var columnHeaderStyle = (XSSFCellStyle)_sheet.Workbook.CreateCellStyle();
            columnHeaderStyle.SetFillForegroundColor(new XSSFColor(Color.LightGray));
            columnHeaderStyle.FillPattern = FillPattern.SolidForeground;
            columnHeaderStyle.SetFont(columnHeaderFont);

            var row = _sheet.GetRow(ColumnHeadersRowIndex);
            if (row == null)
                row = _sheet.CreateRow(ColumnHeadersRowIndex);

            for (var i = 0; i < columnNames.Count(); i++)
            {
                var cell = row.GetCell(i);
                if (cell == null)
                    cell = row.CreateCell(i);

                var name = columnNames.ElementAt(i);
                cell.SetCellValue(name);
                cell.CellStyle = columnHeaderStyle;
            }
        }

        public List<IDataRow> ReadDataRows()
        {
            var columnNames = ReadColumnNames();
            var rowIndex = ColumnHeadersRowIndex + 1;
            var dataRows = new List<IDataRow>();

            while (true)
            {
                var row = _sheet.GetRow(rowIndex);
                if (row == null)
                    break;

                var dataRow = new DataRow(rowIndex);
                bool isBlankRow = true;

                for (var colIndex = 0; colIndex < columnNames.Count; colIndex++)
                {
                    ICell cell = row.GetCell(colIndex);
                    if (cell == null)
                        cell = row.CreateCell(colIndex);

                    var isCellEmpty = IsCellEmpty(cell);

                    if (colIndex == 0 && isCellEmpty)
                        dataRow.IsIndented = true;

                    if (!isCellEmpty)
                        isBlankRow = false;

                    object value = isCellEmpty ? null : GetCellValue(cell);
                    var columnName = columnNames[colIndex];
                    var dataCell = new DataCell(columnName, value, rowIndex, colIndex);
                    dataRow.AddCell(dataCell, columnName);
                }

                if (isBlankRow)
                    break;

                dataRows.Add(dataRow);
                rowIndex++;
            }

            return dataRows;
        }

        public void WriteDataRows(List<Dictionary<string, object>> dataRows)
        {
            var columnNames = ReadColumnNames();
            var rowIndex = ColumnHeadersRowIndex + 1;

            foreach (var dataRow in dataRows)
            {
                rowIndex = WriteDataRow(dataRow, rowIndex, columnNames);
                rowIndex++;
            }
        }

        public void SetCellValue(int rowIndex, int cellIndex, object value)
        {
            var row = _sheet.GetRow(rowIndex);
            if (row == null)
                row = _sheet.CreateRow(rowIndex);

            SetCellValue(row, cellIndex, value);
        }

        public object GetCellValue(int rowIndex, int cellIndex)
        {
            var row = _sheet.GetRow(rowIndex);
            if (row == null)
                return null;

            var cell = row.GetCell(cellIndex);
            if (IsCellEmpty(cell))
                return null;

            var value = GetCellValue(cell);
            return value;
        }

        public void ApplyStyle(object styleReference, int rowIndex, int? cellIndex = null)
        {
            var row = _sheet.GetRow(rowIndex);
            if (row == null)
                row = _sheet.CreateRow(rowIndex);

            if (cellIndex == null)
            {
                row.RowStyle = (ICellStyle)styleReference;
                foreach (var cell in row.Cells)
                    ApplyStyleToCell(styleReference, row, cell.ColumnIndex);
            }
            else
            {
                ApplyStyleToCell(styleReference, row, cellIndex.Value);
            }
        }

        private void ApplyStyleToCell(object styleReference, IRow row, int cellIndex)
        {
            var cell = row.GetCell(cellIndex);
            if (cell == null)
                cell = row.CreateCell(cellIndex);

            cell.CellStyle = (ICellStyle)styleReference;
        }

        private bool IsCellEmpty(ICell cell)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return string.IsNullOrWhiteSpace(cell.StringCellValue);
                    case CellType.Boolean:
                    case CellType.Numeric:
                    case CellType.Formula:
                    case CellType.Error:
                        return false;
                }
            }
            // null, blank or unknown
            return true;
        }

        /// <summary>
        /// Writes the data row (including nested rows) and returns the ending rowIndex after the write operation.
        /// </summary>
        private int WriteDataRow(Dictionary<string, object> dataRow, int writeAtRowIndex, List<string> columnNames)
        {
            var rowIndex = writeAtRowIndex;

            var row = _sheet.GetRow(rowIndex);
            if (row == null)
                row = _sheet.CreateRow(rowIndex);

            foreach (var columnName in dataRow.Keys)
            {
                var value = dataRow[columnName];
                if (value != null)
                {
                    if (value is IEnumerable<Dictionary<string, object>>)
                    {
                        var nestedDataRows = value as IEnumerable<Dictionary<string, object>>;
                        for (var i = 0; i < nestedDataRows.Count(); i++)
                        {
                            if (i > 0)
                                rowIndex++;

                            var nestedDataRow = nestedDataRows.ElementAt(i);
                            rowIndex = WriteDataRow(nestedDataRow, rowIndex, columnNames);
                        }
                    }
                    else
                    {
                        var colIndex = columnNames.IndexOf(columnName);
                        if (colIndex >= 0)
                        {
                            SetCellValue(row, colIndex, value);
                        }
                    }
                }
            }

            return rowIndex;
        }

        private void SetCellValue(IRow row, int cellIndex, object value)
        {
            if (value == null)
                return;

            var cell = row.GetCell(cellIndex);
            if (cell == null)
                cell = row.CreateCell(cellIndex);

            if (value is int i)
                cell.SetCellValue(i);
            else if (value is double d)
                cell.SetCellValue(d);
            else if (value is float f)
                cell.SetCellValue(f);
            else if (value is DateTime dt)
            {
                cell.SetCellValue(dt);

                //Check if value contains time component and assign proper date/time style.
                if (dt > dt.Date)
                    cell.CellStyle = _dateTimeCellStyle;
                else
                    cell.CellStyle = _dateCellStyle;
            }
            else if (value is bool b)
                cell.SetCellValue(b == true ? 1 : 0);
            else if (value is string s)
                cell.SetCellValue(s);
            else
                cell.SetCellValue(value.ToString());
        }

        private object GetCellValue(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Numeric:
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    else
                    {
                        if (cell.NumericCellValue == (int)cell.NumericCellValue)
                            return (int)cell.NumericCellValue;
                        else
                            return cell.NumericCellValue;
                    }
                default:
                    return cell.StringCellValue.Trim();
            }
        }
    }
}
