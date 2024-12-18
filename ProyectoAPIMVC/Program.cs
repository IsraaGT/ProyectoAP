using Microsoft.EntityFrameworkCore;
using ProyectoAPIMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container for API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configura para evitar la serialización cíclica (si es necesario)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configura el DbContext para la base de datos SQL Server
builder.Services.AddDbContext<ProyectoApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

var app = builder.Build();

// Configure the HTTP request pipeline for API
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Servir archivos estáticos desde la carpeta wwwroot
app.UseStaticFiles();

// Redirigir la raíz al archivo login.html
app.MapGet("/", () => Results.Redirect("/login.html"));

// Mapea las rutas para los controladores de la API
app.MapControllers();

app.Run();
