using Microsoft.EntityFrameworkCore;
using BestFilms.Models;
using BestFilms.Controllers;


var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<FilmsContext>(options => options.UseSqlServer(connection));

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
