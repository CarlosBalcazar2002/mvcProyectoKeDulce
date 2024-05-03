using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Modelos.Models;

namespace mvcProyectoKeDulce.Areas.Cliente.Controllers
{

    [Area("Cliente")]
    public class ProductosClientesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public ProductosClientesController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var productos = _contenedorTrabajo.Producto.GetAll();
            return View(productos); // Pasar los productos a la vista
        }
        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Producto.GetAll() });
        }
        #endregion
    }
}
