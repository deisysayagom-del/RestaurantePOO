using Microsoft.EntityFrameworkCore;
using RestaurantePOO.API.Middleware;
using RestaurantePOO.API.Services;
using RestaurantePOO.Core.Interfaces;
using RestaurantePOO.Infrastructure.Persistence;
using RestaurantePOO.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuracion de Entity Framework Core con SQL Server
builder.Services.AddDbContext<RestauranteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyeccion de Dependencias (Repositorios y Servicios)
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();

var app = builder.Build();

// Crear base de datos en SQL Server automaticamente si no existe
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RestauranteDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita archivos estaticos para servir las 3 vistas web desde wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Registro del Middleware Global de Excepciones
app.UseMiddleware<ManejadorExcepcionesMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();