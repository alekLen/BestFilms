using Microsoft.EntityFrameworkCore;
using BestFilms.Models;
using BestFilms.Controllers;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<FilmsContext>(options => options.UseSqlServer(connection));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FilmsContext>();
// ������ NuGet Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
// ��������� ������ ��� ������������� �������� ������
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// ��������� ������� MVC
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
