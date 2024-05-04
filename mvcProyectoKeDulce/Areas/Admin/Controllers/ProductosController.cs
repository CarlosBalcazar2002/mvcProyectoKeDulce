using Microsoft.AspNetCore.Mvc;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Modelos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using NuGet.Protocol.Plugins;
using System;
using System.IO;
using mvcProyectoKeDulce.Modelos.ViewModels;

namespace mvcProyectoKeDulce.Areas.Admin.Controllers
{
    [Authorize(Roles ="Administrador")]

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductoVM artiVM = new ProductoVM()
            {
                Producto = new mvcProyectoKeDulce.Modelos.Models.Producto()
                //ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };


            return View(artiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductoVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (artiVM.Producto.Id == 0 && archivos.Count() > 0)
                {
                    //Nuevo producto
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\productos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Producto.ImagenUrl = @"\imagenes\productos\" + nombreArchivo + extension;
                    //artiVM.Producto.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Producto.Add(artiVM.Producto);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Imagen", "Debes seleccionar una imagen");
                }

            }

            //artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ProductoVM artiVM = new ProductoVM()
            {
                Producto = new mvcProyectoKeDulce.Modelos.Models.Producto(),
                //ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                artiVM.Producto = _contenedorTrabajo.Producto.Get(id.GetValueOrDefault());
            }

            return View(artiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductoVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var productoDesdeBd = _contenedorTrabajo.Producto.Get(artiVM.Producto.Id);


                if (archivos.Count() > 0)
                {
                    //Nuevo imagen para el producto
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\productos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, productoDesdeBd.ImagenUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Producto.ImagenUrl = @"\imagenes\productos\" + nombreArchivo + extension;
                    //artiVM.Producto.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Producto.Update(artiVM.Producto);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aquí sería cuando la imagen ya existe y se conserva
                    artiVM.Producto.ImagenUrl = productoDesdeBd.ImagenUrl;
                }

                _contenedorTrabajo.Producto.Update(artiVM.Producto);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }

            //artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }




        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Producto.GetAll(includeProperties: "Categoria") });
        }

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
