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
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _context;

        public ContenedorTrabajo(ApplicationDbContext context)
        {
            _context = context;
            //se agregan cada uno de los repositorios para que queden encapsulados
            Usuario = new UsuarioRepository(_context);
            Producto = new ProductoRepository(_context);
            SliderProducto = new SliderRepository(_context);
            Venta = new VentaRepository(_context);
            Pedido = new PedidoRepository(_context);
            DetallePedido = new DetallePedidoRepository(_context);

        }
        public IUsuarioRepository Usuario { get; private set; }
        public ISliderRepository SliderProducto { get; private set; }
        public IProductoRepository Producto { get; private set; }
        public IVentaRepository Venta { get; private set; }
        public IPedidoRepository Pedido { get; private set; }
        public IDetallePedidoRepository DetallePedido { get; private set; }




        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
