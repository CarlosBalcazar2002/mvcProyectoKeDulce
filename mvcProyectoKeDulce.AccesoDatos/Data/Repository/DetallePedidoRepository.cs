using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Data;
using mvcProyectoKeDulce.Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository
{
    public class DetallePedidoRepository : Repository<DetallePedido>, IDetallePedidoRepository
    {
        private readonly ApplicationDbContext _db;
        public DetallePedidoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DetallePedido detallePedido)
        {
            var objDesdeDb = _db.DetallePedido.FirstOrDefault(s => s.Id == detallePedido.Id);
            objDesdeDb.PedidoId = detallePedido.PedidoId;
            objDesdeDb.ProductoId = detallePedido.ProductoId;
            objDesdeDb.Cantidad = detallePedido.Cantidad;
            objDesdeDb.PrecioUnitario = detallePedido.PrecioUnitario;
            objDesdeDb.Total = detallePedido.Total;

        }
    }
}
