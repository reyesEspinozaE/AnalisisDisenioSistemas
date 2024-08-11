using IntegraMuni2023._01.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<IntegraMuni2023Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StringConexion")));
builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", config =>
    { config.Cookie.Name = "UserloginCookie"; config.LoginPath = "/Login/Login"; });
var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
