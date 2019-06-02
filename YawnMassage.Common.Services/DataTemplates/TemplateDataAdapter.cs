using YawnMassage.Common.Domain.Contracts.DataTemplates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YawnMassage.Common.Services.DataTemplates
{
    /// <summary>
    /// Represents the default template data adapter which is capable of transforming objects to
    /// data rows and vice versa. Specific data adapters for each object type must inherit from this class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TemplateDataAdapter<T> : ITemplateDataAdapter<T>
    {
        private readonly ITemplateSheetDataStore _store;
        private readonly bool _includesNestedRows;

        public TemplateDataAdapter(ITemplateSheetDataStore store, bool includesNestedRows)
        {
            _store = store;
            _includesNestedRows = includesNestedRows;
        }

        /// <summary>
        /// The inherited adapter should implement this to provide custom conversion behaviour
        /// The given object should be converted into key-value list with keys containing template column names.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>Key value list with data store column names.</returns>
        protected abstract Dictionary<string, object> ConvertObjectToDataRow(T obj);

        /// <summary>
        /// The inherited adapter should implement this to provide custom conversion behaviour.
        /// The given set of data rows should be converted to an object.
        /// </summary>
        protected abstract T ConvertDataRowGroupToObject(IEnumerable<IDataRow> dataRowGroup, Func<Dictionary<string, object>, bool> fieldValidator = null);

        /// <summary>
        /// Reads the data store and converts the data into a list of objects.
        /// </summary>
        public IEnumerable<MappedObject<T>> ReadObjects(out int totalRowCount, Func<Dictionary<string, object>, bool> fieldValidator = null)
        {
            var dataRows = _store.ReadDataRows();
            totalRowCount = dataRows.Count;

            var objects = new List<MappedObject<T>>();

            var dataRowGroup = new List<IDataRow>();

            for (var i = 0; i < dataRows.Count; i++)
            {
                var currentRow = dataRows[i];

                //dataRowGroup.Count == 0 - Always add row considering the row as the first row in the group 
                if (!_includesNestedRows || currentRow.IsIndented || dataRowGroup.Count == 0)
                    dataRowGroup.Add(currentRow);

                var nextRow = (i == dataRows.Count - 1) ? null : dataRows[i + 1];
                if (!_includesNestedRows || nextRow == null || !nextRow.IsIndented)
                {
                    //Call convert method of inherited class to get the object.
                    var obj = ConvertDataRowGroupToObject(dataRowGroup, fieldValidator);
                    //If there were field validation errors, the object would be null.
                    if (obj != null)
                        objects.Add(new MappedObject<T>(obj, dataRowGroup));

                    dataRowGroup = new List<IDataRow>();
                }
            }

            return objects;
        }
        /// <summary>
        /// Writes the specified objects into the underlying data store.
        /// </summary>
        public void WriteObjects(IEnumerable<T> objects)
        {
            var dataRows = new List<Dictionary<string, object>>();

            foreach (var obj in objects)
            {
                //Call convert method of inherited class to get the data row.
                var dataRow = ConvertObjectToDataRow(obj);
                dataRows.Add(dataRow);
            }

            _store.WriteDataRows(dataRows);
        }
    }
}
