using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Template.AppService.Interfaces;
using Template.AppService.Services;
using Template.Domain.Interfaces;
using Template.Domain.Repositories;
using Template.Domain.Services;
using Template.Infra.Repositories;

namespace Template.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            services.AddTransient<IAuthenticateAppService, AuthenticateAppService>();
            services.AddTransient<IAuthenticateService, AuthenticateService>();
            services.AddTransient<IAuthenticateRepository, AuthenticateRepository>();
          
        }
    }
}
