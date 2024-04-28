using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.Modelos.Models
{
    public class SliderProducto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese Un Nombre Para El Slider")]
        [Display(Name = "Nombre Slider")]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }
    }
}
