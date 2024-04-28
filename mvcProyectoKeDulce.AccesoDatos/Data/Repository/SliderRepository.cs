using Microsoft.EntityFrameworkCore;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using mvcProyectoKeDulce.Data;
using mvcProyectoKeDulce.Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository
{

    public class SliderRepository : Repository<SliderProducto>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(DbContext context) : base(context)
        {
        }

        public void Update(SliderProducto sliderProducto)
        {
            var objDesdeDb = _db.SliderProducto.FirstOrDefault(s => s.Id == sliderProducto.Id);
            objDesdeDb.Nombre = sliderProducto.Nombre;
            objDesdeDb.Estado = sliderProducto.Estado;
            objDesdeDb.UrlImagen = sliderProducto.UrlImagen;
        }
    }
}

