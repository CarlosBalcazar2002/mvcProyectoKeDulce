using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Data;
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
        }
        public IUsuarioRepository Usuario { get; private set; }
        public ISliderRepository SliderProducto { get; private set; }

        public IProductoRepository Producto { get; private set; }
        


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
