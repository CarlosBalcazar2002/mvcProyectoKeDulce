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
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public readonly ApplicationDbContext _db;

        public ProductoRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db; 
        }
        public IQueryable<Producto> AsQueryable()
        {
            return _db.Set<Producto>().AsQueryable();
        }
        public void Update (Producto producto)
        {
            var objDesdeDb = _db.Producto.FirstOrDefault(s => s.Id == producto.Id);
            objDesdeDb.NombreProducto = producto.NombreProducto;
            objDesdeDb.Descripcion = producto.Descripcion;
            objDesdeDb.Precio = producto.Precio;
            objDesdeDb.ImagenUrl = producto.ImagenUrl;

            //_db.SaveChanges();
        }
    }
}
