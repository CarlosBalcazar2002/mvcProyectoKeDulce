using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese El Nombre Del Producto")]
        [Display(Name = "Nombre Del Producto")]
        public string NombreProducto { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
        //[DataType(DataType.ImageUrl)]
        //[Display(Name = "image")]
        public int Precio { get; set; }
        //[Display(Name = "Orden de Visualizacion")]
        //[Range(1, 100, ErrorMessage = "El valor debe estar entre 1 y 100")]
        public int ImagenUrl { get; set; }
    }
}
