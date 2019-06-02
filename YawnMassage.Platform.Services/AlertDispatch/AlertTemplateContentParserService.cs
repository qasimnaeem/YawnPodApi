using YawnMassage.Platform.Domain.Contracts.AlertDispatch;
using YawnMassage.Platform.Domain.Documents;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YawnMassage.Platform.Services.AlertDispatch
{
    public class AlertTemplateContentParserService : IAlertTemplateContentParserService
    {
        public IEnumerable<string> GetFillableContextObjectTypes(AlertTemplate template)
        {
            var objectTypes = new HashSet<string>();
            objectTypes.UnionWith(GetFillableContextObjectTypes(template.Subject));
            objectTypes.UnionWith(GetFillableContextObjectTypes(template.Content));
            return objectTypes;
        }

        public void ApplyPlaceholderReplacements(AlertTemplate template, Dictionary<string, string> replacements)
        {
            template.Subject = GetPlaceholderReplacedText(template.Subject, replacements);
            template.Content = GetPlaceholderReplacedText(template.Content, replacements);
        }

        private string GetPlaceholderReplacedText(string text, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            foreach(var placeholder in replacements.Keys)
            {
                var pattern = @"{{\s*?" + placeholder + @"\s*?}}";
                var replacementValue = replacements[placeholder] ?? string.Empty;
                text = Regex.Replace(text, pattern, replacementValue, RegexOptions.IgnoreCase);
            }

            return text;
        }
        
        private HashSet<string> GetFillableContextObjectTypes(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new HashSet<string>();

            var matches = Regex.Matches(text, "{{.*?}}");
            var types = GetContextObjectTypes(matches);
            return types;
        }
        
        private HashSet<string> GetContextObjectTypes(MatchCollection matches)
        {
            var hashSet = new HashSet<string>();

            foreach (Match match in matches)
            {
                var placeholder = match.Value.Trim('{', '}').Trim();
                if (!string.IsNullOrEmpty(placeholder))
                {
                    var placeholderParts = placeholder.Split('.');
                    if (placeholderParts.Length > 1)
                        hashSet.Add(placeholderParts[0].Trim());
                }
            }

            return hashSet;
        }
    }
}
