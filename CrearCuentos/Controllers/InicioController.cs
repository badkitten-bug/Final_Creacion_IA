using Microsoft.AspNetCore.Mvc;

namespace CrearCuentos.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
