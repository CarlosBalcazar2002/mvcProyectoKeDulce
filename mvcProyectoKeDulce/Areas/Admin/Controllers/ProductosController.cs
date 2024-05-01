using Microsoft.AspNetCore.Mvc;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Modelos.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Plugins;

namespace mvcProyectoKeDulce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class ProductosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProductosController(IContenedorTrabajo contenedorTrabajo,
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
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count() > 0)
                {
                    //Nuevo producto
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\productos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    producto.ImagenUrl = @"\imagenes\productos\" + nombreArchivo + extension;
                    _contenedorTrabajo.Producto.Add(producto);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
                }

            }
            return View(producto);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var producto = _contenedorTrabajo.Producto.Get(id.GetValueOrDefault());
                return View(producto);
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var productoDdesdeBd = _contenedorTrabajo.Producto.Get(producto.Id);

                if (archivos.Count() > 0)
                {
                    //Nuevo imagen producto
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\productos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    //var nuevaExtension = Path.GetExtension(archivos[0].FileName);
                    var rutaImagen = Path.Combine(rutaPrincipal, productoDdesdeBd.ImagenUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    producto.ImagenUrl = @"\imagenes\productos\" + nombreArchivo + extension;
                    _contenedorTrabajo.Producto.Update(producto);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aquí sería cuando la imagen ya existe y se conserva
                    producto.ImagenUrl = productoDdesdeBd.ImagenUrl;
                }
                _contenedorTrabajo.Producto.Update(producto);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }
        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        { return Json(new { data = _contenedorTrabajo.Producto.GetAll() }); }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productoDesdeBd = _contenedorTrabajo.Producto.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, productoDesdeBd.ImagenUrl.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            if (productoDesdeBd == null)
            {
                return Json(new { success = false, message = "Error borrando producto" });
            }
            _contenedorTrabajo.Producto.Remove(productoDesdeBd);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Producto Borrado Correctamente" });
        }
        #endregion
    }

}