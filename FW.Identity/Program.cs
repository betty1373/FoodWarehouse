using FW.Identity;
using FW.Identity.Data;
using FW.Identity.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("DbConnectionString");

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(connectionString);
})
   .AddIdentity<ApplicationUser, ApplicationRole>(options =>
   {
       options.Password.RequiredLength = 6;
       options.Password.RequireDigit = false;
       options.Password.RequireNonAlphanumeric = false;
       options.Password.RequireUppercase = false;
       options.Lockout.MaxFailedAccessAttempts = 5;
       options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
       options.User.RequireUniqueEmail = true;
       options.SignIn.RequireConfirmedEmail = false;
   })
   .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
    //options.IssuerUri = "https://fwidentity:10001";
})
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryClients(Configuration.Clients)
    .AddAspNetIdentity<ApplicationUser>()
    .AddJwtBearerClientAuthentication()
    .AddDeveloperSigningCredential();

/*builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "FW.Identity.Cookie";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});*/

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}
app.Run();
