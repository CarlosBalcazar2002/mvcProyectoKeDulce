using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvcProyectoKeDulce.Modelos.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }


        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}
