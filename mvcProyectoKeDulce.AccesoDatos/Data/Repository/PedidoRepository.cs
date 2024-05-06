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
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _db;
        public PedidoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Pedido pedido)
        {
            var objDesdeDb = _db.Pedido.FirstOrDefault(s => s.Id == pedido.Id);
            objDesdeDb.UsuarioId = pedido.UsuarioId;
            objDesdeDb.Fecha = pedido.Fecha;
            objDesdeDb.Estado = pedido.Estado;
        }
    }
}
