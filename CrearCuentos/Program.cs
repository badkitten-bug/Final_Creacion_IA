using CrearCuentos.Models;
using Microsoft.AspNetCore.Authentication.Cookies; // A�ade este namespace para configurar la autenticaci�n de cookies
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios a la colecci�n de servicios.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CrearCuentosContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("conexion"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.21-mariadb")));

// Agregar servicios de sesi�n
builder.Services.AddDistributedMemoryCache(); // Usa la memoria para almacenar datos de sesi�n
builder.Services.AddSession();

// Configurar la autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Sesion/Login"; // Establece la ruta de inicio de sesi�n
        options.AccessDeniedPath = "/Sesion/Login"; // Establece la ruta de inicio de sesi�n
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 d�as. Puede cambiarlo para escenarios de producci�n, consulte https://aka.ms/aspnetcore-hsts.
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

// Habilitar el middleware de autenticaci�n
app.UseAuthentication();

// Habilitar el middleware de autorizaci�n
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");

app.Run();
