using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts.BulkData;
using YawnMassage.Common.Domain.Contracts.DataTemplates;
using YawnMassage.Common.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services.BulkData
{
    /// <summary>
    /// Provides common functionality for importing all types of bulk data objects.
    /// All object type specific importers should inherit from this class.
    /// </summary>
    public abstract class ObjectImporterBase<T> : IBulkDataImporter<T>
    {
        private IBulkDataTagService _groupTagService;
        private bool _validateGroupTag;

        protected ObjectImporterBase(IBulkDataTagService groupTagService, bool validateGroupTag = true)
        {
            _groupTagService = groupTagService;
            _validateGroupTag = validateGroupTag;
        }

        protected abstract Task ImportObjectToDatabaseAsync(T obj);
        
        protected abstract ITemplateDataAdapter<T> GetDataAdapter(ITemplateSheetDataStore datastore, Dictionary<string, string> columnMappings);

        protected virtual void BeforeImport(T obj)
        {
        }

        protected virtual void AfterImport(T obj)
        {
        }

        public async Task<ImportResult> ImportAsync(IEnumerable<string> includedGroupIds,
            ITemplateSheetDataStore datastore, Dictionary<string, string> columnMappings)
        {
            var dataAdapter = GetDataAdapter(datastore, columnMappings);
            var totalRowCount = 0;

            var mappedObjects = dataAdapter.ReadObjects(out totalRowCount, fields => //Field validation function
            {
                if (!_validateGroupTag)
                    return true;

                //Check whether this object belongs to a group that is inside the scope of included groups.
                var groupTag = (string)fields["grouptag"];
                var isValid = _groupTagService.TagExists(groupTag);
                return isValid;
            });

            //Perform any further validations for mappedObjects.
            foreach (var obj in mappedObjects)
            {
                ValidateMappedObject(obj);
            }

            //Check whether there are any errors before begining actual import to database.
            var errorRows = GetErrorRows(mappedObjects);
            if (errorRows.Any())
                return new ImportResult { ErrorRows = errorRows, TotalProcessedRowCount = totalRowCount };

            var importedObjectsCount = 0;

            //Perform import of all objects and record any errors.
            foreach (var mappedObject in mappedObjects)
            {
                try
                {
                    BeforeImport(mappedObject.Object); //Allows custom code execution before import.
                    await ImportObjectToDatabaseAsync(mappedObject.Object);
                    AfterImport(mappedObject.Object); //Allows custom code execution after import.

                    importedObjectsCount++;
                }
                catch (YawnMassageException ex)
                {
                    var dataRow = mappedObject.DataRowGroup.First();
                    dataRow.MarkAsError(ex.Message);
                }
            }

            errorRows = GetErrorRows(mappedObjects);
            
            return new ImportResult
            {
                TotalProcessedRowCount = totalRowCount,
                ErrorRows = errorRows,
                SuccessObjectsCount = importedObjectsCount,
            };
        }

        protected virtual void ValidateMappedObject(MappedObject<T> mappedObject)
        {
            //Implement custom validattion logic in inherited class.
        }

        private IEnumerable<ErrorRow> GetErrorRows(IEnumerable<MappedObject<T>> mappedObjects)
        {
            var errorRows = mappedObjects.SelectMany(m => m.DataRowGroup).Where(r => r.HasError)
                .Select(r => new ErrorRow
                {
                    RowIndex = r.RowIndex,
                    ErrorMessage = r.ErrorMessage,
                    ErrorCells = r.Cells.Where(c => c.HasRequiredError || c.HasDataFormatError)
                });

            return errorRows;
        }
    }
}
