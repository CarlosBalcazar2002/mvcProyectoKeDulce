using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository;
using mvcProyectoKeDulce.Data;
using mvcProyectoKeDulce.Modelos.Models;
using mvcProyectoKeDulce.AccesoDatos.Data.Inicializador;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new 
    InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//modificar para que se administre el manejo de roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

//agregar contenedor de trabajo al contenedos IoC de inyeccion de dependencias
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

//Siembra de datos - Paso 1
builder.Services.AddScoped<IInicializadorBD, InicializadorBD>();

// Agregar servicio de sesión
builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//Siembra de datos - Paso 2 Metodo que ejecuta la siembra de datos
SiembraDatos();

app.UseRouting();

// Habilitar el uso de sesiones
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

//Funcionalidad metodo SiembraDeDatos();
void SiembraDatos()
{
    using (var scope = app.Services.CreateScope())
    {
        var inicializadorBD = scope.ServiceProvider.GetRequiredService<IInicializadorBD>();
        inicializadorBD.Inicializar();
    }
}
