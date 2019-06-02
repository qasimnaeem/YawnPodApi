using System.Drawing;
using System.IO;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Represents a file which contains multiple spreadsheets.
    /// </summary>
    public interface ITemplateFile
    {
        /// <summary>
        /// The GetTemplateSheetDataStore
        /// </summary>
        /// <param name="sheetIndex">The sheetIndex<see cref="int"/></param>
        /// <param name="columnHeadersRowNumber">The columnHeadersRowNumber<see cref="int"/></param>
        /// <returns>The <see cref="ITemplateSheetDataStore"/></returns>
        ITemplateSheetDataStore GetTemplateSheetDataStore(int sheetIndex, int columnHeadersRowNumber);

        /// <summary>
        /// The GetTemplateSheetDataStore
        /// </summary>
        /// <param name="sheetName">The sheetName<see cref="string"/></param>
        /// <param name="columnHeadersRowNumber">The columnHeadersRowNumber<see cref="int"/></param>
        /// <returns>The <see cref="ITemplateSheetDataStore"/></returns>
        ITemplateSheetDataStore GetTemplateSheetDataStore(string sheetName, int columnHeadersRowNumber);

        /// <summary>
        /// The CreateTemplateSheetDataStore
        /// </summary>
        /// <param name="sheetName">The sheetName<see cref="string"/></param>
        /// <param name="columnHeadersRowNumber">The columnHeadersRowNumber<see cref="int"/></param>
        /// <returns>The <see cref="ITemplateSheetDataStore"/></returns>
        ITemplateSheetDataStore CreateTemplateSheetDataStore(string sheetName, int columnHeadersRowNumber);

        /// <summary>
        /// The CreateStyleReference
        /// </summary>
        /// <param name="textColor">The textColor<see cref="Color?"/></param>
        /// <param name="backgroundColor">The backgroundColor<see cref="Color?"/></param>
        /// <returns>The <see cref="object"/></returns>
        object CreateStyleReference(Color? textColor = null, Color? backgroundColor = null);

        /// <summary>
        /// The Write
        /// </summary>
        /// <param name="s">The s<see cref="Stream"/></param>
        void Write(Stream s);
    }
}
