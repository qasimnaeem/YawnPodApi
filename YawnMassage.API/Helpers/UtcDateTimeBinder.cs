using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace YawnMassage.Api.Helpers
{
    public class UtcDateTimeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
            {
                return Task.CompletedTask;
            }

            DateTime datetime;
            if (DateTime.TryParse(valueProviderResult.FirstValue, null, DateTimeStyles.AdjustToUniversal, out datetime))
            {
                bindingContext.Result = ModelBindingResult.Success(datetime);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    bindingContext.ModelMetadata
                    .ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                      valueProviderResult.ToString(), nameof(DateTime)));
            }

            return Task.CompletedTask;
        }
    }
}
