using YawnMassage.Common.Domain.Contracts.DataTemplates;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;
using System.IO;

namespace YawnMassage.Common.Services.DataTemplates
{
    /// <summary>
    /// Wraps an Excel workbook file.
    /// </summary>
    public class TemplateFile : ITemplateFile
    {
        private readonly IWorkbook _workbook;

        public TemplateFile()
        {
            _workbook = new XSSFWorkbook();
        }

        public TemplateFile(Stream stream)
        {
            _workbook = new XSSFWorkbook(stream);
        }

        public ITemplateSheetDataStore GetTemplateSheetDataStore(int sheetIndex, int columnHeadersRowNumber)
        {
            var sheet = _workbook.GetSheetAt(sheetIndex);

            if (sheet == null)
                return null;

            var templateSheet = new TemplateSheetDataStore(sheet, columnHeadersRowNumber);
            return templateSheet;
        }

        public ITemplateSheetDataStore GetTemplateSheetDataStore(string sheetName, int columnHeadersRowNumber)
        {
            var sheet = _workbook.GetSheet(sheetName);

            if (sheet == null)
                return null;

            var templateSheet = new TemplateSheetDataStore(sheet, columnHeadersRowNumber);
            return templateSheet;
        }

        public ITemplateSheetDataStore CreateTemplateSheetDataStore(string sheetName, int columnHeadersRowNumber)
        {
            var sheet = _workbook.CreateSheet(sheetName);
            var templateSheet = new TemplateSheetDataStore(sheet, columnHeadersRowNumber);
            return templateSheet;
        }

        public object CreateStyleReference(Color? textColor = null, Color? backgroundColor = null)
        {
            var style = (XSSFCellStyle)_workbook.CreateCellStyle();

            if (textColor != null)
            {
                var font = (XSSFFont)_workbook.CreateFont();
                font.SetColor(new XSSFColor(textColor.Value));
                style.SetFont(font);
            }

            if (backgroundColor != null)
            {
                style.SetFillForegroundColor(new XSSFColor(backgroundColor.Value));
                style.FillPattern = FillPattern.SolidForeground;
            }

            return style;
        }

        public void Write(Stream s)
        {
            _workbook.Write(s);
        }
    }
}
