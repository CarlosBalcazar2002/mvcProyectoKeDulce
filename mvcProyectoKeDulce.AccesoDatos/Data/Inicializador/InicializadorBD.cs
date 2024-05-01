using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvcProyectoKeDulce.Data;
using mvcProyectoKeDulce.Modelos.Models;
using mvcProyectoKeDulce.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Inicializador
{
    public class InicializadorBD : IInicializadorBD

    {
        private readonly ApplicationDbContext _bd;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public InicializadorBD(ApplicationDbContext bd, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _bd = bd;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Inicializar()
        {
            try
            {
                if (_bd.Database.GetPendingMigrations().Count() > 0)
                {
                    _bd.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_bd.Roles.Any(ro => ro.Name == Roles.Administrador)) return;


            _roleManager.CreateAsync(new IdentityRole(Roles.Administrador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Registrado)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Cliente)).GetAwaiter().GetResult();

            // Creación del usuario inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "diegovalverde@gmail.com",
                Email = "diegovalverde@gmail.com",
                EmailConfirmed = true,
                Nombre = "diego valverde",
                
                

            }, "Aa1234567*").GetAwaiter().GetResult();

            ApplicationUser user = _bd.ApplicationUser.Where(u => u.Email == "diegovalverde@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, Roles.Administrador).GetAwaiter().GetResult();

            // Verificar si ya existen productos en la base de datos
            if (!_bd.Producto.Any())
            {
                // Creación de productos iniciales
                var productosEjemplo = new List<Producto>
                {
                    new Producto
                    {
                        NombreProducto = "Donnuts",
                        Descripcion = "Deliciosos donnuts con glaceado de chocolate.",
                        Precio = 8,
                        ImagenUrl = "/imagenes/productos/donas.jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Torta de chocolate",
                        Descripcion = "Torta para 20 personas sabor chocolate.",
                        Precio = 90,
                        ImagenUrl = "/imagenes/productos/tortas.jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Manzanas Acarameladas",
                        Descripcion = "Manzanas bañadas en caramelo.",
                        Precio = 5,
                        ImagenUrl = "/imagenes/productos/manzanas-acarameladas.jpg"
                    }
                };

                _bd.Producto.AddRange(productosEjemplo);
                _bd.SaveChanges();
            }
        }
    }
}
