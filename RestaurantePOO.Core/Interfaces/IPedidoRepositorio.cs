using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantePOO.Core.Entities.Pedidos;
using RestaurantePOO.Core.Enums;

namespace RestaurantePOO.Core.Interfaces
{
    public interface IPedidoRepositorio : IRepositorioBase<Pedido>
    {
        Task<IEnumerable<Pedido>> ObtenerPorEstadoAsync(EstadoPedido estado);
        Task<Pedido?> ObtenerDetalleCompletoAsync(int numeroPedido);
    }
}
