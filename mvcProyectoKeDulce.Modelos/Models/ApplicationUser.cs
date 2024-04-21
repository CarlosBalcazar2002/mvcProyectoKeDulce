using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
