using CrearCuentos.Models;
using Microsoft.AspNetCore.Authentication.Cookies; // Añade este namespace para configurar la autenticación de cookies
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios a la colección de servicios.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CrearCuentosContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("conexion"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.21-mariadb")));

// Agregar servicios de sesión
builder.Services.AddDistributedMemoryCache(); // Usa la memoria para almacenar datos de sesión
builder.Services.AddSession();

// Configurar la autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Sesion/Login"; // Establece la ruta de inicio de sesión
        options.AccessDeniedPath = "/Sesion/Login"; // Establece la ruta de inicio de sesión
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 días. Puede cambiarlo para escenarios de producción, consulte https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//+app.UseForwardedHeaders();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
//app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions { RequestPath = "/" });
//app.UsePathBase("/");
// Habilitar el uso de sesiones
app.UseSession();

// Habilitar el middleware de autenticación
app.UseAuthentication();

// Habilitar el middleware de autorización
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");

app.Run();
