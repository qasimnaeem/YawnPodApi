using System.Net;
using System.Threading.Tasks;
using YawnMassage.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using YawnMassage.Api;
using YawnMassage.Api.Helpers;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Services.Factories;
using YawnMassage.Platform.Services;
using YawnMassage.Platform.Services.Configuration;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Identity.Documents;
using YawnMassage.Common.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using YawnMassage.Api.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;

namespace YawnMassage.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbSettingsConfigurationSection = Configuration.GetSection("DbConnection");
            DbInfo appSettingsDbInfo = new DbInfo();
            dbSettingsConfigurationSection.Bind(appSettingsDbInfo);

            var queueConfigurationSection = Configuration.GetSection("QueueNames");
            QueueNameInfo appSettingsQueueNameInfo = new QueueNameInfo();
            queueConfigurationSection.Bind(appSettingsQueueNameInfo);

            //Singletons
            services.AddSingleton(s => appSettingsDbInfo);
            services.AddSingleton(s => appSettingsQueueNameInfo);
            services.AddSingleton<IDocumentDbService, CosmosDbService>();
            //services.AddSingleton<ILoggerService, ILoggerService>();

            //Scoped
            services.AddScoped<RequestContext>();
            services.AddScoped<IUserContextService, WebUserContextService>();
            services.AddScoped<IGroupDataContextFactory, GroupDataContextFactory>();
            services.AddScoped<IGroupDataContext, GroupDataContext>();
            services.AddScoped<ISystemDataContext, SystemDataContext>();
            services.AddScoped<IEventsDbService, EventsDbService>();
            services.AddScoped<IConfigurationReaderService, ConfigurationReaderService>();
            services.AddScoped<ILocalisationReaderService, LocalisationReaderService>();
            services.AddScoped<ILookupReaderService, LookupReaderService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ILocalisationService, LocalisationService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            //services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipleFactory>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipleFactory>();
            services.AddScoped<IPlatformServiceBusService, PlatformServiceBusService>();
            services.AddScoped<IBulkDataService, BulkDataService>();
            services.AddScoped<IGroupPermissionService, GroupPermissionService>();
            services.AddScoped<IServiceBusService, AzureServiceBusService>();
            services.AddScoped<IBlobServiceFactory, BlobServiceFactory>();
            services.AddScoped<IAlertNotificationRequestService, AlertNotificationRequestService>();
            services.AddScoped<IAlertTemplateService, AlertTemplateService>();
            services.AddIdentity<User, DocumentDbIdentityRole>()
                .AddDocumentDbStores()
                .AddDefaultTokenProviders();
           
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();

            //Register the in-memory cookie auth ticket store.
            //This is to avoid large auth cookie when there are lot of claims.
            //Note: This will only work for single-instance server setup.
            services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, InjectedAuthenticationOptions>();
            services.AddSingleton<ITicketStore, InMemoryTicketStore>();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "CloudYawnPod";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.FromResult(0);
                };

                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.FromResult(0);
                };
            });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.ModelBinderProviders.Insert(0, new DateTimeBinderProvider());
                options.AllowCombiningAuthorizeFilters = false;
            })
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 });

            services.AddSwaggerGen(swagger =>
            {
                swagger.DescribeAllEnumsAsStrings();
                swagger.DescribeAllParametersInCamelCase();
                swagger.SwaggerDoc("v1", new Info { Title = "YawnMassage Platform API", Version = "v1" });
                swagger.OperationFilter<SwaggerCustomHeaderParameter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(o =>
            {
                o.PreSerializeFilters.Add((document, request) =>
                {
                    document.Paths = document.Paths.ToDictionary(p => p.Key.ToLowerInvariant(), p => p.Value);
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "YawnMassage API V1");
            });

        }
    }
}
