using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvcProyectoKeDulce.Modelos.Models;

namespace mvcProyectoKeDulce.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Poner aqui los modelos que se vayan creando
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<SliderProducto> SliderProducto { get; set; }
    }

}
