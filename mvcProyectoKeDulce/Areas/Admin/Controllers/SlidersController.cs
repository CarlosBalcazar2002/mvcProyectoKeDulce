using Microsoft.AspNetCore.Mvc;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Modelos.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Plugins;

namespace mvcProyectoKeDulce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SlidersController(IContenedorTrabajo contenedorTrabajo,
            IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult Index() { return View(); }
        [HttpGet]
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SliderProducto slider)

        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count() > 0)
                {
                    //Nuevo slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                    _contenedorTrabajo.SliderProducto.Add(slider);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
                }

            }
            return View(slider);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var slider = _contenedorTrabajo.SliderProducto.Get(id.GetValueOrDefault());

                return View(slider);
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(SliderProducto slider)

        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;


                var sliderDdesdeBd = _contenedorTrabajo.SliderProducto.Get(slider.Id);


                if (archivos.Count() > 0)
                {
                    //Nuevo imagen slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    //var nuevaExtension = Path.GetExtension(archivos[0].FileName);
                    var rutaImagen = Path.Combine(rutaPrincipal, sliderDdesdeBd.UrlImagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                    _contenedorTrabajo.SliderProducto.Update(slider);

                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aquí sería cuando la imagen ya existe y se conserva
                    slider.UrlImagen = sliderDdesdeBd.UrlImagen;
                }

                _contenedorTrabajo.SliderProducto.Update(slider);

                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }
        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()

        { return Json(new { data = _contenedorTrabajo.SliderProducto.GetAll() }); }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sliderDesdeBd = _contenedorTrabajo.SliderProducto.Get(id);

            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, sliderDesdeBd.UrlImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            if (sliderDesdeBd == null)
            {
                return Json(new { success = false, message = "Error borrando slider" });
            }

            _contenedorTrabajo.SliderProducto.Remove(sliderDesdeBd);

            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider Borrado Correctamente" });
        }
        #endregion
    }

}