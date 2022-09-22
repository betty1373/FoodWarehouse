using FW.Web.Configurations.Options;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FW.Web.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authentication = configuration.GetSection(AuthenticationOptions.KeyValue).Get<AuthenticationOptions>();

            // добавляет службы аутентификации в DI и настраивает Bearer как схему по умолчанию.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                .AddIdentityServerAuthentication(options =>
                {
                    options.ApiName = authentication.ApiName;
                    options.Authority = authentication.Authority;
                    options.RequireHttpsMetadata = false;
                });

            //Добавим код, который позволяет проверять наличие области действия в токене доступа, который клиент запросил (и получил).
            //Для этого мы будем использовать систему политик авторизации ASP.NET Core.
            //Теперь вы можете применять эту политику на различных уровнях, например: - глобально, - для всех конечных точек API, - для конкретных контроллеров/действий
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "scopeWebAPI");
                });
            });
        }
    }
}
