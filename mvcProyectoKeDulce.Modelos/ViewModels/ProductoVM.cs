using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using mvcProyectoKeDulce.Modelos.Models;

namespace mvcProyectoKeDulce.Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }

    }
}
