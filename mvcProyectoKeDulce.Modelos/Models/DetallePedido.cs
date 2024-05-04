using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcProyectoKeDulce.Modelos.Models
{
    public class DetallePedido
    {
        [Key]
        public int Id { get; set; }


        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }


        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El total es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
    }
}
