using YawnMassage.Common.Domain.Contracts.DataTemplates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YawnMassage.Common.Services.DataTemplates
{
    /// <summary>
    /// A special data adapter which is capable of transforming internal field names
    /// into template column names and vice versa.
    /// </summary>
    public abstract class ColumnMappedTemplateDataAdapter<T> : TemplateDataAdapter<T>
    {
        private readonly ITemplateSheetDataStore _store;
        private readonly Dictionary<string, string> _friendlyColumnNames;
        private readonly Dictionary<string, string> _internalColumnNames;

        public ColumnMappedTemplateDataAdapter(ITemplateSheetDataStore store, Dictionary<string, string> columnMappings,
            bool includesNestedRows) : base(store, includesNestedRows)
        {
            _store = store;

            //columnMappings is a dictionary with { internalName => friendlyName } mappings.
            _friendlyColumnNames = columnMappings.ToDictionary(m => m.Key.ToLower(), m => m.Value);
            _internalColumnNames = columnMappings.ToDictionary(m => m.Value.ToLower(), m => m.Key);
        }

        /// <summary>
        /// The inerited classs must implement this method to provide custom conversion behaviour.
        /// It should convert the given fieldset containing internal field names into an object.
        /// </summary>
        /// <param name="fieldSets">Key-value list with internal field names as keys.</param>
        /// <returns>The converted object.</returns>
        protected abstract T ConvertFieldsToObject(IEnumerable<MappedFieldSet> fieldSets);

        /// <summary>
        /// The inherited class must implement this method to provide custom conversion behaviour.
        /// it should convert the given object into a key-value list with keys containing internal field names.
        /// </summary>
        protected abstract Dictionary<string, object> ConvertObjectToFields(T obj);

        protected override T ConvertDataRowGroupToObject(IEnumerable<IDataRow> dataRowGroup, Func<Dictionary<string, object>, bool> fieldValidator = null)
        {
            //Convert datarows into fieldsets with internal field names and calls the inerited conversion method.
            var fieldSets = new List<MappedFieldSet>();
            foreach (var dataRow in dataRowGroup)
            {
                var fieldCells = dataRow.Cells.ToDictionary(c => GetInternalName(c.ColumnName.ToLower()));
                var fieldSet = new MappedFieldSet(dataRow, fieldCells);
                fieldSets.Add(fieldSet);
            }

            //Convert the first fieldSet to simple key-value dictionary and pass to validate function.
            var primaryFieldSetValues = fieldSets.First().FieldCells.ToDictionary(d => d.Key, d => d.Value.Value);
            var isFieldsValid = fieldValidator == null ? true : fieldValidator(primaryFieldSetValues);

            if (!isFieldsValid)
                return default(T);

            //Call the inherited class method to actually convert the fieldset to the object.
            var obj = ConvertFieldsToObject(fieldSets);
            return obj;
        }

        protected override Dictionary<string, object> ConvertObjectToDataRow(T obj)
        {
            //Call the inherited class method to actually convert the object into the field set.
            var fields = ConvertObjectToFields(obj);
            var dataRow = fields.ToDictionary(f => GetFriendlyName(f.Key.ToLower()), f => GetProcessedValue(f.Value));
            return dataRow;
        }
        
        private object GetProcessedValue(object value)
        {
            if (value is IEnumerable<Dictionary<string, object>>)
            {
                var nestedFieldsList = value as IEnumerable<Dictionary<string, object>>;
                var newValue = nestedFieldsList.Select(dic => dic.ToDictionary(
                    f => GetFriendlyName(f.Key.ToLower()),
                    f => GetProcessedValue(f.Value)));

                return newValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Get template column name for a give internal field name.
        /// </summary>
        private string GetFriendlyName(string internalName)
        {
            string name = null;
            if (_friendlyColumnNames.ContainsKey(internalName))
                name = _friendlyColumnNames[internalName];
            else
                name = internalName;
            return name.ToLower();
        }

        /// <summary>
        /// Get internal field name for a give template column name.
        /// </summary>
        private string GetInternalName(string friendlyName)
        {
            string name = null;
            if (_internalColumnNames.ContainsKey(friendlyName))
                name = _internalColumnNames[friendlyName];
            else
                name = friendlyName;
            return name.ToLower();
        }
    }
}
