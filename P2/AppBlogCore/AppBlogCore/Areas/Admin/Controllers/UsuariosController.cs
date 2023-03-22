using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace AppBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //private readonly ApplicationDbContext _context;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
            //_context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //Opcion 1: Obtener todos los usuarios
            //return View(_contenedorTrabajo.Usuario.GetAll());


            //Opcion 2: Obtener todos los usuarios menos el que este logueado, para no bloquearse el mismo
            var ClaimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));





        }

        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if(id== null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
