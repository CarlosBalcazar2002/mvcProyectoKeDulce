using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcProyectoKeDulce.Modelos.Models
{
    public enum EstadoPedido
    {
        Pendiente,
        Anulado,
        Realizado
    }
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        public ICollection<DetallePedido> DetallesPedidos { get; set; }

        [Required(ErrorMessage = "El estado del pedido es obligatorio")]
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente; // Valor por defecto: Pendiente
    
  }
}
