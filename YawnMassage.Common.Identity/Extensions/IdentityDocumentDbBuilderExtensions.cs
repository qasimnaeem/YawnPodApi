using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using YawnMassage.Common.Identity.Stores;
using YawnMassage.Common.Identity.Support;

namespace YawnMassage.Common.Identity.Extensions
{
    public static class IdentityDocumentDbBuilderExtensions
    {
        public static IdentityBuilder AddDocumentDbStores(this IdentityBuilder builder)
        {
            // TODO: Until DocumentDB SDK exposes it's JSON.NET settings, we need to hijack the global settings to serialize claims
            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings()
                {
                    Converters = new List<JsonConverter>() { new JsonClaimConverter(), new JsonClaimsPrincipalConverter(), new JsonClaimsIdentityConverter() }
                };
            };
            
            builder.Services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(builder.RoleType),
                typeof(DocumentDbRoleStore<>).MakeGenericType(builder.RoleType));

            builder.Services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(builder.UserType),
                typeof(DocumentDbUserStore<,>).MakeGenericType(builder.UserType, builder.RoleType));

            builder.Services.AddTransient<ILookupNormalizer, LookupNormalizer>();

            return builder;
        }
    }
}
