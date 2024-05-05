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

            // Creación del primer usuario
            if (!_userManager.Users.Any(u => u.Email == "diegovalverde@gmail.com"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "diegovalverde@gmail.com",
                    Email = "diegovalverde@gmail.com",
                    EmailConfirmed = true,
                    Nombre = "diego valverde"
                };

                var result = _userManager.CreateAsync(adminUser, "Aa1234567*").GetAwaiter().GetResult();

            }, "Aa1234567*").GetAwaiter().GetResult();

            ApplicationUser user = _bd.ApplicationUser.Where(u => u.Email == "diegovalverde@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, Roles.Administrador).GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(otroUsuario, Roles.Administrador).GetAwaiter().GetResult();
                }
            }

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
                        NombreProducto = "Donuts",
                        Descripcion = "Donuts decorados con chocolate y glaciados (Cantidad y Color aelección) 🤤",
                        Precio = 5,
                        ImagenUrl = "/imagenes/productos/Donuts (1).jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Torta de chocolate",
                        Descripcion = "Torta para 20 personas sabor chocolate con decorado de frutillas.",
                        Precio = 100,
                        ImagenUrl = "/imagenes/productos/Tortas(2).jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Manzanas Acarameladas",
                        Descripcion = "Manzanas bañadas en caramelo (Cantidad a Eleccion).",
                        Precio = 5,
                        ImagenUrl = "/imagenes/productos/Manzanas (1).jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Torta De Limon",
                        Descripcion = "Torta de limon bañado con chocolate y crema.",
                        Precio = 5,
                        ImagenUrl = "/imagenes/productos/Donuts (1).jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Frutillas",
                        Descripcion = "Fresas cubiertas con chocolate + rosas naturales ( color a elección) 😘",
                        Precio = 14,
                        ImagenUrl = "/imagenes/productos/Frutillas (1).jpg"
                    },
                    new Producto
                    {
                        NombreProducto = "Tortas",
                        Descripcion = "Tortas para 20 personas Con decoración de Frutillas y Relleno De Crema",
                        Precio = 5,
                        ImagenUrl = "/imagenes/productos/Tortas(1).jpg"
                    }
                };
                _bd.Producto.AddRange(productosEjemplo);
                _bd.SaveChanges();
            }
        }
    }
}