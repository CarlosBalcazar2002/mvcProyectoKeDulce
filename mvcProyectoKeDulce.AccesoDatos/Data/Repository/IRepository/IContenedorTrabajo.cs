using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        IUsuarioRepository Usuario { get; }
        IProductoRepository Producto { get; }
        ISliderRepository SliderProducto { get; }
        IVentaRepository Venta { get; }
        IPedidoRepository Pedido { get; }
        IDetallePedidoRepository DetallePedido { get; }

        void Save();
    }
}
