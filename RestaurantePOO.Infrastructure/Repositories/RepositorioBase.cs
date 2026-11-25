using Microsoft.EntityFrameworkCore;
using RestaurantePOO.Core.Exceptions;
using RestaurantePOO.Core.Interfaces;
using RestaurantePOO.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RestaurantePOO.Infrastructure.Repositories
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        protected readonly RestauranteDbContext _context;

        public RepositorioBase(RestauranteDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> ObtenerPorIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task AgregarAsync(T entidad)
        {
            try
            {
                await _context.Set<T>().AddAsync(entidad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ReglaNegocioException($"Error al guardar en SQL Server ({typeof(T).Name}): verifique que el identificador no esté duplicado. Detalle: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"[Persistencia] Operación AgregarAsync finalizada para {typeof(T).Name}.");
            }
        }

        public virtual async Task ActualizarAsync(T entidad)
        {
            try
            {
                _context.Set<T>().Update(entidad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ReglaNegocioException($"Error al actualizar en SQL Server ({typeof(T).Name}): {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"[Persistencia] Operación ActualizarAsync finalizada para {typeof(T).Name}.");
            }
        }

        public virtual async Task EliminarAsync(object id)
        {
            try
            {
                var entidad = await ObtenerPorIdAsync(id);
                if (entidad != null)
                {
                    _context.Set<T>().Remove(entidad);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new ReglaNegocioException($"No se puede eliminar {typeof(T).Name} porque tiene registros asociados. Detalle: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"[Persistencia] Operación EliminarAsync finalizada para {typeof(T).Name}.");
            }
        }
    }
}
