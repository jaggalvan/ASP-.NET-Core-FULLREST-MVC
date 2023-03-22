using AppBlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppBlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            //_logger = logger;
            _contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM()
            {
                Slider = _contenedorTrabajo.Slider.GetAll(),
                ListaArticulos = _contenedorTrabajo.Articulo.GetAll()
            };

            //Esta linea es para poder saber si estamos en el home o en el inicio
            ViewBag.IsHome = true;


            return View(homeVm);
        }

        public IActionResult Details(int id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            return View(articuloDesdeDb);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}