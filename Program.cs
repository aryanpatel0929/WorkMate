using AttendanceManagement.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange:true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllersWithViews();
IDatabaseConnection databaseConnection = new DatabaseConnection(builder.Configuration);
builder.Services.AddTransient<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddMemoryCache();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
