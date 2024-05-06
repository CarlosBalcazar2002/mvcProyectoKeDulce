using mvcProyectoKeDulce.Modelos.Models;
using System;
using System.Linq;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository
{
    public interface IProductoRepository : IRepository<Producto>
    {
        IQueryable<Producto> AsQueryable();
        void Update(Producto producto);
    }
}
