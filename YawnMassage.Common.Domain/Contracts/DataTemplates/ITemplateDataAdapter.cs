using System;
using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Contracts.DataTemplates
{
    /// <summary>
    /// Defines the <see cref="ITemplateDataAdapter{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITemplateDataAdapter<T>
    {
        /// <summary>
        /// The ReadObjects
        /// </summary>
        /// <param name="totalRowCount">The totalRowCount<see cref="int"/></param>
        /// <param name="fieldValidator">The fieldValidator<see cref="Func{Dictionary{string, object}, bool}"/></param>
        /// <returns>The <see cref="IEnumerable{MappedObject{T}}"/></returns>
        IEnumerable<MappedObject<T>> ReadObjects(out int totalRowCount, Func<Dictionary<string, object>, bool> fieldValidator = null);

        /// <summary>
        /// The WriteObjects
        /// </summary>
        /// <param name="objects">The objects<see cref="IEnumerable{T}"/></param>
        void WriteObjects(IEnumerable<T> objects);
    }
}
