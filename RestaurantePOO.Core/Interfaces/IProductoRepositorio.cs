using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantePOO.Core.Entities.Productos;

namespace RestaurantePOO.Core.Interfaces
{
    public interface IProductoRepositorio : IRepositorioBase<Producto>
    {
        Task<IEnumerable<Producto>> ObtenerDisponiblesAsync();
    }
}
