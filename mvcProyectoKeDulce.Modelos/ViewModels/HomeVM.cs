using mvcProyectoKeDulce.Modelos.Models;
using mvcProyectoKeDulce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.Modelos.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SliderProducto> Sliders { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
}
