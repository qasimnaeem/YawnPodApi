using System.Collections.Generic;
using YawnMassage.Platform.Domain.Documents;
namespace YawnMassage.Platform.Domain.Contracts.AlertDispatch
{
    /// <summary>
    /// Defines the <see cref="IAlertTemplateContentParserService" />
    /// </summary>
    public interface IAlertTemplateContentParserService
    {
        /// <summary>
        /// The GetFillableContextObjectTypes
        /// </summary>
        /// <param name="template">The template<see cref="AlertTemplate"/></param>
        /// <returns>The <see cref="IEnumerable{string}"/></returns>
        IEnumerable<string> GetFillableContextObjectTypes(AlertTemplate template);

        /// <summary>
        /// The ApplyPlaceholderReplacements
        /// </summary>
        /// <param name="template">The template<see cref="AlertTemplate"/></param>
        /// <param name="replacements">The replacements<see cref="Dictionary{string, string}"/></param>
        void ApplyPlaceholderReplacements(AlertTemplate template, Dictionary<string, string> replacements);
    }
}
