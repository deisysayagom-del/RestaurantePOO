using Microsoft.EntityFrameworkCore;
using RestaurantePOO.Core.Entities.Pedidos;
using RestaurantePOO.Core.Entities.Productos;
using RestaurantePOO.Core.Enums;
using RestaurantePOO.Core.Exceptions;
using RestaurantePOO.Core.Interfaces;
using RestaurantePOO.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantePOO.Infrastructure.Repositories
{
    public class PedidoRepositorio : RepositorioBase<Pedido>, IPedidoRepositorio
    {
        public PedidoRepositorio(RestauranteDbContext context) : base(context) { }

        public override async Task<IEnumerable<Pedido>> ObtenerTodosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(p => p.Numero)
                .ToListAsync();
        }

        public override async Task AgregarAsync(Pedido entidad)
        {
            try
            {
                foreach (var detalle in entidad.Detalles)
                {
                    if (detalle.Producto != null)
                    {
                        _context.Entry(detalle.Producto).State = EntityState.Unchanged;
                    }
                }

                if (entidad is PedidoMesa pedidoMesa && pedidoMesa.Mesa != null)
                {
                    var mesaExistente = await _context.Mesas.FindAsync(pedidoMesa.Mesa.Numero);
                    if (mesaExistente != null)
                    {
                        _context.Entry(pedidoMesa.Mesa).State = EntityState.Unchanged;
                    }
                }

                await _context.Set<Pedido>().AddAsync(entidad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ReglaNegocioException($"No se pudo registrar el pedido #{entidad.Numero}. Verifique que el número no esté duplicado. Detalle: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"[Persistencia] AgregarAsync finalizado para Pedido #{entidad.Numero}.");
            }
        }

        public async Task<IEnumerable<Pedido>> ObtenerPorEstadoAsync(EstadoPedido estado)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(p => p.Estado == estado)
                .ToListAsync();
        }

        public async Task<Pedido?> ObtenerDetalleCompletoAsync(int numeroPedido)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(prod => ((Bebida)prod).InsumoDirecto)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(prod => ((Plato)prod).Receta)
                            .ThenInclude(ing => ing.Insumo)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(prod => ((Combo)prod).Componentes)
                .FirstOrDefaultAsync(p => p.Numero == numeroPedido);
        }
    }
}
