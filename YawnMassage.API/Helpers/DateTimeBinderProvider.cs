using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace YawnMassage.Api.Helpers
{
    public class DateTimeBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return new UtcDateTimeBinder();
            }
            return null; // TODO: Find alternate.  
        }
    }
}
