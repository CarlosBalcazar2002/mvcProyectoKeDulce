using Microsoft.EntityFrameworkCore;
using mvcProyectoKeDulce.AccesoDatos.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace mvcProyectoKeDulce.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            //se crea una consulta IQueryable a partir del Dbset del contexto
            IQueryable<T> query = dbSet;

            //Se Aplica el filtro si se pronuncia
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //se icluyen propiedades de navegacion si se proporcionan 
            if (includeProperties != null)
            {
                //Se divide la cadena de propiedades por coma y se itera sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            //se aplica ordenamiento si se proporciona 
            if (orderBy != null)
            {
                //se ejecuta la funcion de ordenamiento y se convierte la consulta en una lista
                return orderBy(query).ToList();
            }
            //si no se proporciona ordenamiento, simplemente se convierte la consulta en una lista
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            // se crea una consulta IQueryable a partir del Dbset de contexto
            IQueryable<T> query = dbSet;

            //se aplica el filtro si se proporciona 
            if (filter != null)
            {
                query = query.Where(filter);
            }
            //se incluyen las propiedades de navegacion si se proporcionan 
            if (includeProperties != null)
            {
                //Se divide la cadena de propiedades por coma y se itera sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
