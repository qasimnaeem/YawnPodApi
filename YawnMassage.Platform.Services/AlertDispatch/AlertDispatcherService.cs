using YawnMassage.Platform.Domain.Contracts.AlertDispatch;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Dto.Alerts;
using YawnMassage.Common.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertDispatch
{
    public class AlertDispatcherService : IAlertDispatcherService
    {
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly ISystemDataContext _systemDataContext;
        private readonly IAlertTemplateContentParserService _alertContentParserService;
        private readonly IAlertContextObjectServiceFactory _alertContextObjectServiceFactory;
        private readonly IAlertSenderServiceFactory _alertSenderServiceFactory;
        private readonly RequestContext _requestContext;

        public AlertDispatcherService(IGroupDataContextFactory groupDataContextFactory, ISystemDataContext systemDataContext,
            IAlertTemplateContentParserService alertContentParserService, IAlertContextObjectServiceFactory alertContextObjectServiceFactory,
            IAlertSenderServiceFactory alertSenderServiceFactory, RequestContext requestContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _systemDataContext = systemDataContext;
            _alertContentParserService = alertContentParserService;
            _alertContextObjectServiceFactory = alertContextObjectServiceFactory;
            _alertSenderServiceFactory = alertSenderServiceFactory;
            _requestContext = requestContext;
        }

        public async Task DispatchAlertAsync(AlertNotificationRequestDto alertReq)
        {
            var recipientUser = await _systemDataContext.GetDocumentAsync<User>(alertReq.RecipientUserId);

            var groupId = _requestContext.GroupId;
            var culture = recipientUser.Culture;
            var timezone = recipientUser.TimeZone;

            var template = await GetTemplateWithFallbackAsync(alertReq.TemplateKey, groupId, culture, alertReq.ChannelKey);
            if (template == null)
                throw new YawnMassageException("TEMPLATE_NOT_FOUND");

            await MergeTemplateWithMasterTemplateAsync(template, groupId, culture);

            //Prepare template placeholder replacements.
            var templateValueReplacements = await GetTemplateValueReplacements(alertReq, groupId, culture, timezone, template);

            //Replace placeholder values.
            _alertContentParserService.ApplyPlaceholderReplacements(template, templateValueReplacements);

            //Send the alert.
            var alertSenderService = _alertSenderServiceFactory.GetAlertSenderService(alertReq.ChannelKey);
            await alertSenderService.SendAsync(template.Content, template.Subject, template.SenderId, recipientUser);
        }

        private async Task<Dictionary<string, string>> GetTemplateValueReplacements(AlertNotificationRequestDto alertReq, string groupId, string culture, string timezone, AlertTemplate template)
        {
            if (alertReq.ContextObjectInfos == null)
                alertReq.ContextObjectInfos = new Dictionary<string, object>();

            //Inject default system context for all alerts.
            alertReq.ContextObjectInfos.Add(AlertObjectContextTypes.System, null);

            var templateValueReplacements = await GetContextValueReplacementsAsync(template, alertReq.ContextObjectInfos, groupId, culture, timezone);
            if (alertReq.CustomParamReplacements != null)
            {
                foreach (var kv in alertReq.CustomParamReplacements)
                    templateValueReplacements.Add(kv.Key, kv.Value);
            }

            return templateValueReplacements;
        }

        /// <summary>
        /// Construct template placeholder replacements from object context values.
        /// </summary>
        private async Task<Dictionary<string, string>> GetContextValueReplacementsAsync(AlertTemplate template, Dictionary<string, object> contextObjectInfos, string groupId, string culture, string timezone)
        {
            //Find out the context objects types that needs to used in placeholder replacements
            //and only load the values for those object types. (Ignore any object types that are unused for this template)

            //Get object types mentioned in the template content placeholders.
            var templateContextObjectTypes = _alertContentParserService.GetFillableContextObjectTypes(template);
            //Get object types that are in both provided context infos and the template.
            var objectTypesToResolve = contextObjectInfos.Keys.Intersect(templateContextObjectTypes);

            var replacements = new Dictionary<string, string>();
            foreach (var objType in objectTypesToResolve)
            {
                var objectInfo = contextObjectInfos[objType];
                var objectService = _alertContextObjectServiceFactory.GetAlertContextObjectService(objType);
                var objectValues = await objectService.GetContextObjectKeyValuesAsync(objectInfo, groupId, culture, timezone);

                foreach (var field in objectValues.Keys)
                {
                    var value = objectValues[field];
                    replacements.Add($"{objType}.{field}", value);
                }
            }

            return replacements;
        }

        private async Task MergeTemplateWithMasterTemplateAsync(AlertTemplate childTemplate, string groupId, string culture)
        {
            var masterTemplate = await GetTemplateWithFallbackAsync(SystemKeys.AlertTemplateKeys.Master, culture, culture, childTemplate.Channel);
            if (childTemplate == null)
                return;

            //Replace master template subject/body placeholders with content from the child template.
            _alertContentParserService.ApplyPlaceholderReplacements(masterTemplate, new Dictionary<string, string>
            {
                { MasterAlertTemplatePlaceholders.Subject, childTemplate.Subject },
                { MasterAlertTemplatePlaceholders.Body, childTemplate.Content }
            });

            //Assign resulting content back to the child template.
            childTemplate.Subject = masterTemplate.Subject;
            childTemplate.Content = masterTemplate.Content;

            if (string.IsNullOrEmpty(childTemplate.SenderId))
                childTemplate.SenderId = masterTemplate.SenderId;
        }

        private async Task<AlertTemplate> GetTemplateWithFallbackAsync(string key, string groupId, string culture, string channel)
        {
            var template = await GetTemplateAsync(key, groupId, culture, channel);
            //Fallback to "*" group if no template was found for specific group.
            if (template == null && _requestContext.GroupId != "*")
                template = await GetTemplateAsync(key, "*", culture, channel);

            return template;
        }

        private async Task<AlertTemplate> GetTemplateAsync(string key, string groupId, string culture, string channel)
        {
            IGroupDataContext dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

            var text = await dataContext.FirstOrDefaultAsync<AlertTemplate, AlertTemplate>(q =>
                                q.Where(c =>
                                    (c.Culture == culture || c.Culture == "*")
                                    && c.Section == "*"
                                    && c.Key == key
                                    && c.Channel == channel)
                                .OrderBy(c => c.Priority));

            return text;
        }
    }
}
