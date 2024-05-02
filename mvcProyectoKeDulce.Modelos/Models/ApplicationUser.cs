using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace mvcProyectoKeDulce.Modelos.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El Nombre Es Obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La Direccion Es Obligatorio")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La Ciudad Es Obligatoria")]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El Celular Es Obligatorio")]

        public string Celular { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Venta> Ventas { get; set; }
    }
}
