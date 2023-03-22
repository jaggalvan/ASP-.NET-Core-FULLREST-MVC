using AppBlogCore.Data;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            return View(artivm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (artiVM.Articulo.Id == 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var filestreams = new FileStream(Path.Combine(subidas, nombreArchivo +extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(filestreams);

                    }

                    artiVM.Articulo.UrlImage = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Add(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));


                }
                

            }
            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                artivm.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }
            return View(artivm);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;


                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);

                if (archivos.Count() >0)
                {

                    //Nueva imagen para el articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImage.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }


                    // nuevamente subimos el archivo

                    using (var filestreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(filestreams);

                    }

                    artiVM.Articulo.UrlImage = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));


                }
                else
                {
                    //Aqui seria cuando la imagen ya existe y se conserva
                    artiVM.Articulo.UrlImage = articuloDesdeDb.UrlImage;

                }


                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }

            return View(artiVM);
 
        }








        #region Llamadas a la API

        [HttpGet]
        public IActionResult GetAll()
        {
            //opcion 1
            return Json(new { Data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImage.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrado articulo" });
            }


            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Articulo borrado correctamente" });
        }







        #endregion
    }
}
