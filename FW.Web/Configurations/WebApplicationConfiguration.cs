using Swashbuckle.AspNetCore.SwaggerUI;

namespace FW.Web.Configurations
{
    public static class WebApplicationConfiguration
    {
        public static WebApplication Configure(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodWarehouse API v1");
                    options.DocumentTitle = "Title";
                    options.RoutePrefix = "docs";
                    options.DocExpansion(DocExpansion.List);
                    options.OAuthClientId("clientWebApi");
                    options.OAuthScopeSeparator(" ");
                    options.OAuthClientSecret("36a4d0df-d361-4c3c-a3eb-2e519d4c4391");
                });
            }
            app.UseHttpsRedirection()
               .UseRouting()
               .UseCors("DefaultPolicy")
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();
               });

            return app;
        }
    }
}
