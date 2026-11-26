using Microsoft.EntityFrameworkCore;
using RestaurantePOO.Core.Entities.Productos;
using RestaurantePOO.Core.Exceptions;
using RestaurantePOO.Core.Interfaces;
using RestaurantePOO.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantePOO.Infrastructure.Repositories
{
    public class ProductoRepositorio : RepositorioBase<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio(RestauranteDbContext context) : base(context) { }

        public override async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos
                .Include(p => ((Bebida)p).InsumoDirecto)
                .Include(p => ((Plato)p).Estacion)
                .Include(p => ((Plato)p).Receta)
                    .ThenInclude(i => i.Insumo)
                .Include(p => ((Combo)p).Componentes)
                .ToListAsync();
        }

        public override async Task<Producto?> ObtenerPorIdAsync(object id)
        {
            if (id is not string codigo) return null;

            return await _context.Productos
                .Include(p => ((Bebida)p).InsumoDirecto)
                .Include(p => ((Plato)p).Estacion)
                .Include(p => ((Plato)p).Receta)
                    .ThenInclude(i => i.Insumo)
                .Include(p => ((Combo)p).Componentes)
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<IEnumerable<Producto>> ObtenerDisponiblesAsync()
        {
            var productos = await ObtenerTodosAsync();
            return productos.Where(p => p.Disponibilidad() > 0).ToList();
        }

        public override async Task AgregarAsync(Producto entidad)
        {
            try
            {
                if (entidad is Bebida bebida && bebida.InsumoDirecto != null)
                {
                    bool insumoExiste = await _context.Insumos
                        .AsNoTracking()
                        .AnyAsync(i => i.Nombre == bebida.InsumoDirecto.Nombre);

                    if (insumoExiste)
                    {
                        _context.Entry(bebida.InsumoDirecto).State = EntityState.Unchanged;
                    }
                }

                if (entidad is Plato plato)
                {
                    if (plato.Estacion != null)
                    {
                        bool estacionExiste = await _context.Estaciones
                            .AsNoTracking()
                            .AnyAsync(e => e.Nombre == plato.Estacion.Nombre);

                        if (estacionExiste)
                        {
                            _context.Entry(plato.Estacion).State = EntityState.Unchanged;
                        }
                    }

                    foreach (var ingrediente in plato.Receta)
                    {
                        if (ingrediente.Insumo != null)
                        {
                            bool insumoExiste = await _context.Insumos
                                .AsNoTracking()
                                .AnyAsync(i => i.Nombre == ingrediente.Insumo.Nombre);

                            if (insumoExiste)
                            {
                                _context.Entry(ingrediente.Insumo).State = EntityState.Unchanged;
                            }
                        }
                    }
                }

                await _context.Set<Producto>().AddAsync(entidad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ReglaNegocioException($"No se pudo guardar el producto '{entidad.Codigo}'. Verifique que el código no esté duplicado. Detalle: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}