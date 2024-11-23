using Appointr.DomainProfile;
using Appointr.Extentions;
using Appointr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

//------------------------[ Add services to the container ]-----------------------------
void ConfigureServices(IServiceCollection services)
{
    // Add services to the container.
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddDbContext<AppDbContext>(options =>
    {
        //---------------------------[ Database Connection ]------------------------------------
        options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection"));
        //--------------------------------------------------------------------------------------
    });
    services.AddAutoMapper(typeof(MappingProfile));
    services.AddRepositories();
    services.AddUnitOfWork();
    services.AddServices();
    services.AddHelpers();
}
//--------------------------------------------------------------------------------------

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=dashboard}/{action=index}/{id?}"
);
app.UseHttpLog();
app.UseConcurrentRequest(30);
app.Run();