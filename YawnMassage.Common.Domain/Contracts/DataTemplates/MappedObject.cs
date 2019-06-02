using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Represents a C# object which has been constructed from a set of data rows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappedObject<T>
    {
        /// <summary>
        /// Gets the Object
        /// The object which can be imported to the database.
        /// </summary>
        public T Object { get; }

        /// <summary>
        /// Gets the DataRowGroup
        /// The source set of data rows used to construct this object.
        /// </summary>
        public IEnumerable<IDataRow> DataRowGroup { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedObject{T}"/> class.
        /// </summary>
        /// <param name="obj">The obj<see cref="T"/></param>
        /// <param name="dataRowGroup">The dataRowGroup<see cref="IEnumerable{IDataRow}"/></param>
        public MappedObject(T obj, IEnumerable<IDataRow> dataRowGroup)
        {
            Object = obj;
            DataRowGroup = dataRowGroup;
        }
    }
}
