using Microsoft.EntityFrameworkCore;
using ShopDevelop.Data.DataBase;
using ShopDevelop.Data.TRASHER.InsertDb;
using ShopDevelop.Service;
using ShopDevelop.Data.Models;
/*using ShopDevelop.Data.Entity;*/

var builder = WebApplication.CreateBuilder(args);
// ��������� ������������ � �������������.
builder.Services.AddControllersWithViews();
// �������� ������ ����������� �� ����� ������������ (appsettings.json).
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
// ���������� Entity Framework Core � �������� �������.
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseNpgsql(connection);
    });
    /*.AddIdentity<User, ApplicationRole>(config => 
    {
        config.Password.RequireDigit = false;
        config.Password.RequireLowercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
        config.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
// ���������� Cookie � �������� �������.
builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", config =>
    {
        config.LoginPath = "/Account/Login";
    });

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
});
// ��������� AddDistributedMemoryCache.
builder.Services.AddDistributedMemoryCache();
// ��������� ������� ������.
builder.Services.AddSession();

var app = builder.Build();

// ����������� ������ ��� ���������� �������� � ���� ������.
DbObjects.Initial(app);

app.Configuration.Bind("Project", new Config());

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// ��������� ������.
app.UseSession();
// ��������� �������������.
app.UseRouting();
// ��������� ����������� ������.
app.UseStaticFiles();
// ��������� ��������������.
app.UseAuthentication();
// ��������� �����������.
app.UseAuthorization();
// ��������� cookie.
app.UseCookiePolicy();

// ������������ ������ ��� ��������.
app.MapControllerRoute(
    name: "product",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Item}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=UserProfile}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cart}/{action=Index}/{id?}");

app.Run();