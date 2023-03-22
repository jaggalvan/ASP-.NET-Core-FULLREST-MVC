using AppBlogCore.Data;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SlidersController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
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

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                    //Nuevo slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var filestreams = new FileStream(Path.Combine(subidas, nombreArchivo +extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(filestreams);

                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                 

                    _contenedorTrabajo.Slider.Add(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));


                

            }
           
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {


            if (id != null)
            {
                var slider = _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
                return View();
            }
            return View();

        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;


                var sliderDesdeDb = _contenedorTrabajo.Articulo.Get(slider.Id);

                if (archivos.Count() >0)
                {

                    //Nueva imagen para el slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImage.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }


                    // nuevamente subimos el archivo

                    using (var filestreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(filestreams);

                    }

                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                    

                    _contenedorTrabajo.Slider.Update(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));


                }
                else
                {
                    //Aqui seria cuando la imagen ya existe y se conserva
                    slider.UrlImagen = sliderDesdeDb.UrlImage;

                }
                _contenedorTrabajo.Slider.Update(slider);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }

            return View();
        }








        #region Llamadas a la API

        [HttpGet]
        public IActionResult GetAll()
        {
            //opcion 1
            return Json(new { Data = _contenedorTrabajo.Slider.GetAll() });
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var sliderDesdeDb = _contenedorTrabajo.Slider.Get(id);


            if (sliderDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrado slider" });
            }


            _contenedorTrabajo.Slider.Remove(sliderDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider borrado correctamente" });
        }







        #endregion
    }
}
