using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Modelos.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class ClienteProductosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteProductosController(IContenedorTrabajo contenedorTrabajo, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult List()
        {
            var productos = _contenedorTrabajo.Producto.GetAll();
            return View(productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AgregarAlCarrito(int id, int cantidad = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var producto = _contenedorTrabajo.Producto.Get(id);

            if (producto == null)
            {
                return NotFound();
            }

            var carrito = _httpContextAccessor.HttpContext.Session.GetString("Carrito");
            var productosEnCarrito = string.IsNullOrEmpty(carrito) ? new List<Producto>() : JsonConvert.DeserializeObject<List<Producto>>(carrito);

            for (int i = 0; i < cantidad; i++)
            {
                productosEnCarrito.Add(producto); // Agregar el objeto completo
            }

            _httpContextAccessor.HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(productosEnCarrito));

            return RedirectToAction("List");
        }


        public IActionResult Carrito()
        {
            var carrito = _httpContextAccessor.HttpContext.Session.GetString("Carrito");
            var productosEnCarrito = string.IsNullOrEmpty(carrito) ? new List<Producto>() : JsonConvert.DeserializeObject<List<Producto>>(carrito);

            // Agrupar productos por ID y sumar las cantidades
            var productosAgrupados = productosEnCarrito
                .GroupBy(p => p.Id)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    NombreProducto = g.First().NombreProducto,
                    Precio = g.First().Precio,
                    Cantidad = g.Count(), // Sumar las cantidades de los productos
                    Total = g.First().Precio * g.Count() // Calcular el total
                })
                .ToList();

            ViewData["CantidadTotal"] = productosAgrupados.Sum(p => p.Cantidad); // Calcular la cantidad total
            ViewData["TotalGeneral"] = productosAgrupados.Sum(p => p.Total); // Calcular el total general

            return View(productosAgrupados);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult EliminarDelCarrito(int id)
        {
            try
            {
                var carrito = _httpContextAccessor.HttpContext.Session.GetString("Carrito");
                var productosEnCarrito = string.IsNullOrEmpty(carrito) ? new List<Producto>() : JsonConvert.DeserializeObject<List<Producto>>(carrito);

                // Eliminar todos los productos con el mismo ID
                productosEnCarrito.RemoveAll(p => p.Id == id);

                // Guardar el carrito actualizado en la sesión
                _httpContextAccessor.HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(productosEnCarrito));

                return RedirectToAction("Carrito");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult VaciarCarrito()
        {
            try
            {
                // Verificar si existe la clave "Carrito" en la sesión
                if (_httpContextAccessor.HttpContext.Session.Keys.Contains("Carrito"))
                {
                    // Eliminar el contenido del carrito de la sesión
                    _httpContextAccessor.HttpContext.Session.Remove("Carrito");
                }

                // Redireccionar a la vista del carrito
                return RedirectToAction("Carrito");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmarPedido()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Unauthorized();
                }

                var carrito = _httpContextAccessor.HttpContext.Session.GetString("Carrito");
                var productosEnCarrito = string.IsNullOrEmpty(carrito) ? new List<Producto>() : JsonConvert.DeserializeObject<List<Producto>>(carrito);

                // Agrupar productos por ID y sumar las cantidades
                var productosAgrupados = productosEnCarrito
                    .GroupBy(p => p.Id)
                    .Select(g => new
                    {
                        ProductoId = g.Key,
                        Cantidad = g.Count(), // Sumar las cantidades de los productos
                        Producto = _contenedorTrabajo.Producto.Get(g.Key) // Cargar explícitamente el producto asociado
                    })
                    .ToList();

                // Crear un nuevo pedido
                var pedido = new Pedido
                {
                    UsuarioId = user.Id,
                    Fecha = DateTime.Now,
                    Estado = EstadoPedido.Pendiente,
                    DetallesPedidos = new List<DetallePedido>()
                };

                // Crear detalles del pedido agrupados por producto
                foreach (var productoAgrupado in productosAgrupados)
                {
                    var detallePedido = new DetallePedido
                    {
                        ProductoId = productoAgrupado.ProductoId,
                        Producto = productoAgrupado.Producto, // Asignar el producto cargado
                        Cantidad = productoAgrupado.Cantidad,
                        PrecioUnitario = productoAgrupado.Producto.Precio, // Utilizar el precio del producto
                        Total = productoAgrupado.Producto.Precio * productoAgrupado.Cantidad // Calcular el total
                    };
                    pedido.DetallesPedidos.Add(detallePedido);
                }

                // Guardar el pedido y los detalles en la base de datos
                _contenedorTrabajo.Pedido.Add(pedido);
                _contenedorTrabajo.Save();

                // Limpiar el carrito de la sesión
                _httpContextAccessor.HttpContext.Session.Remove("Carrito");

                return View("ConfirmarPedido", pedido);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
