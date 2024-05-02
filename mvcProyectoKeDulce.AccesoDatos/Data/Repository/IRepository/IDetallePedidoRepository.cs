using mvcProyectoKeDulce.Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository
{

    public interface IDetallePedidoRepository : IRepository<DetallePedido>
    {
        void Update(DetallePedido detallePedido);
    }
}

