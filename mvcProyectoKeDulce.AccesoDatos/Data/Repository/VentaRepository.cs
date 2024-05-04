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
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        private readonly ApplicationDbContext _db;
        public VentaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Venta venta)
        {
            var objDesdeDb = _db.Venta.FirstOrDefault(s => s.Id == venta.Id);
            objDesdeDb.PedidoId = venta.PedidoId;
            objDesdeDb.UsuarioId = venta.UsuarioId;
        }
    }
}
