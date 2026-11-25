using Microsoft.EntityFrameworkCore;
using RestaurantePOO.Core.Entities.Productos;
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

        public async Task<IEnumerable<Producto>> ObtenerDisponiblesAsync()
        {
            var productos = await _context.Productos.ToListAsync();
            // Filtrar usando la lógica polimórfica de Disponibilidad() del dominio
            return productos.Where(p => p.Disponibilidad() > 0);
        }
    }
}
