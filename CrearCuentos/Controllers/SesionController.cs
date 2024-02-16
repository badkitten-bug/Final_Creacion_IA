using CrearCuentos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//1.- REFERENCES AUTHENTICATION COOKIE
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CrearCuentos.Controllers
{
    public class SesionController : Controller
    {
        private readonly CrearCuentosContext _context;

        public SesionController(CrearCuentosContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var user = _context.Usuarios
                    .SingleOrDefault(u => u.NombreDeUsuario == usuario.NombreDeUsuario && u.Contraseña == usuario.Contraseña);

            if (user != null)
            {
                //2.- CONFIGURACION DE LA AUTENTICACION
                #region AUTENTICACTION
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.NombreCompletos),
                    new Claim("Correo", user.Correo),
                    new Claim("PermisoInicial", user.PermisoInicial.ToString()), // Agregar PermisoInicial
                    new Claim("PermisoPrimaria", user.PermisoPrimaria.ToString()), // Agregar PermisoPrimaria
                    new Claim("PermisoSecundaria", user.PermisoSecundaria.ToString()) // Agregar PermisoSecundaria
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                #endregion
                if(user.Id == 9)
                {
                    return RedirectToAction("Index", "Usuarios");
                }

                return RedirectToAction("Cuento", "Generar");
                
            }
            else
            {
                ModelState.AddModelError("", "Credenciales incorrectas.");
            }

            return View(usuario);
        }

        public async Task<IActionResult> Salir()
        {
            //3.- CONFIGURACION DE LA AUTENTICACION
            #region AUTENTICACTION
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            return RedirectToAction("Login");

        }

    }
}
