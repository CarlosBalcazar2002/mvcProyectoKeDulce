using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.Modelos.Models
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

        // Cambia el tipo de datos de Precio a decimal
        [Required(ErrorMessage = "El precio es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)] // Formato de moneda
        public decimal Precio { get; set; }

        // Cambia el tipo de datos de ImagenUrl a string
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string ImagenUrl { get; set; }
    }
}
