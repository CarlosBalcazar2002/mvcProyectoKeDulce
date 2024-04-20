using mvcProyectoKeDulce.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository
{
    public interface IProductoRepository : IRepository<Producto>
    {
        void Update(Producto producto);
    }
}
