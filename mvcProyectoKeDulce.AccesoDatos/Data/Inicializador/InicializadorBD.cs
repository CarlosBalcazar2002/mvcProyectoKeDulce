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

        //Creamos el constructor
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

            //Creación de roles
            _roleManager.CreateAsync(new IdentityRole(Roles.Administrador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Registrado)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Cliente)).GetAwaiter().GetResult();

            //Creación del usuario inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "nia20bh@gmail.com",
                Email = "nia20bh@gmail.com",
                EmailConfirmed = true,
                Nombre = "Carlos Eduardo"
            }, "Lafamilia_2002").GetAwaiter().GetResult();

            ApplicationUser user = _bd.ApplicationUser.Where(u => u.Email == "nia20bh@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, Roles.Administrador).GetAwaiter().GetResult();

            //Cupones de descuento para cualquiera de mis cursos: joseandresmont@gmail.com
        }
    }
}
