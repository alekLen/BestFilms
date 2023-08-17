using Microsoft.EntityFrameworkCore;
using BestFilms.Models;
using BestFilms.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<FilmsContext>(options => options.UseSqlServer(connection));

// качаем NuGet Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
// добавляем сервис для динамического создания вьюшек
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
