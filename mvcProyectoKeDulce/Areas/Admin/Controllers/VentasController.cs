using Microsoft.AspNetCore.Mvc;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Modelos.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;



namespace mvcProyectoKeDulce.Areas.Ventas.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class VentasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public VentasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            var pedidos = _contenedorTrabajo.Pedido.GetAll(includeProperties: "Usuario"); // Obtener todos los pedidos incluyendo la información del usuario asociado
            return View(pedidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstado(int id, EstadoPedido estado)
        {
            var pedido = _contenedorTrabajo.Pedido.GetFirstOrDefault(p => p.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            pedido.Estado = estado;

            if (estado == EstadoPedido.Anulado || estado == EstadoPedido.Realizado)
            {
                // Obtener el ID del usuario actual
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Crear una nueva venta
                var venta = new Venta
                {
                    UsuarioId = userId,
                    PedidoId = pedido.Id
                };

                // Agregar la venta a la base de datos
                _contenedorTrabajo.Venta.Add(venta);
            }

            _contenedorTrabajo.Save();

            return RedirectToAction("Index");
        }

        public IActionResult ReporteVentas()
        {
            var ventas = _contenedorTrabajo.Venta.GetAll(includeProperties: "Usuario,Pedido.Usuario,DetallesPedidos.Producto"); // Incluir los datos del usuario asociado al pedido y a la venta
            return View(ventas);
        }

        public IActionResult VerDetallePedido(int id)
        {
            // Obtener el pedido de la base de datos incluyendo los detalles del pedido y el usuario asociado
            var pedido = _contenedorTrabajo.Pedido.GetFirstOrDefault(
                p => p.Id == id,
                includeProperties: "DetallesPedidos.Producto,Usuario"
            );

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }





    }
}
