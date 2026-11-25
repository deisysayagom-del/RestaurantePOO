using RestaurantePOO.API.DTOs;
using RestaurantePOO.Core.Entities;
using RestaurantePOO.Core.Entities.Pedidos;
using RestaurantePOO.Core.Exceptions;
using RestaurantePOO.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantePOO.API.Services
{
    public interface IPedidoServicio
    {
        Task<IEnumerable<Pedido>> ObtenerTodosAsync();
        Task<Pedido> ObtenerPorNumeroAsync(int numero);
        Task<PedidoMesa> CrearPedidoMesaAsync(CrearPedidoMesaRequest request);
        Task<PedidoParaLlevar> CrearPedidoParaLlevarAsync(CrearPedidoParaLlevarRequest request);
        Task<PedidoDomicilio> CrearPedidoDomicilioAsync(CrearPedidoDomicilioRequest request);
        Task<Pedido> AvanzarEstadoAsync(int numero);
    }

    public class PedidoServicio : IPedidoServicio
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IProductoRepositorio _productoRepositorio;

        public PedidoServicio(IPedidoRepositorio pedidoRepositorio, IProductoRepositorio productoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _productoRepositorio = productoRepositorio;
        }

        public async Task<IEnumerable<Pedido>> ObtenerTodosAsync()
        {
            return await _pedidoRepositorio.ObtenerTodosAsync();
        }

        public async Task<Pedido> ObtenerPorNumeroAsync(int numero)
        {
            var pedido = await _pedidoRepositorio.ObtenerDetalleCompletoAsync(numero);
            if (pedido == null)
                throw new EntidadNoEncontradaException($"No se encontró el pedido número '{numero}'.");

            return pedido;
        }

        public async Task<PedidoMesa> CrearPedidoMesaAsync(CrearPedidoMesaRequest request)
        {
            var mesa = new Mesa(request.NumeroMesa, request.CapacidadMesa);
            var pedido = new PedidoMesa(request.NumeroPedido, mesa);

            await AgregarItemsAPedidoAsync(pedido, request.Items);
            await _pedidoRepositorio.AgregarAsync(pedido);
            return pedido;
        }

        public async Task<PedidoParaLlevar> CrearPedidoParaLlevarAsync(CrearPedidoParaLlevarRequest request)
        {
            var pedido = new PedidoParaLlevar(request.NumeroPedido, request.NombreCliente, request.EmpaqueEspecial, request.HoraEstimadaRecogida);

            await AgregarItemsAPedidoAsync(pedido, request.Items);
            await _pedidoRepositorio.AgregarAsync(pedido);
            return pedido;
        }

        public async Task<PedidoDomicilio> CrearPedidoDomicilioAsync(CrearPedidoDomicilioRequest request)
        {
            var repartidor = new Repartidor(request.NombreRepartidor, request.TelefonoRepartidor);
            var zona = new ZonaReparto(request.NombreZona, request.CostoEnvio, request.TiempoEstimadoMinutos);
            var pedido = new PedidoDomicilio(request.NumeroPedido, request.Direccion, repartidor, zona);

            await AgregarItemsAPedidoAsync(pedido, request.Items);
            await _pedidoRepositorio.AgregarAsync(pedido);
            return pedido;
        }

        public async Task<Pedido> AvanzarEstadoAsync(int numero)
        {
            var pedido = await _pedidoRepositorio.ObtenerDetalleCompletoAsync(numero);
            if (pedido == null)
                throw new EntidadNoEncontradaException($"Pedido '{numero}' no encontrado.");

            pedido.AvanzarEstado();
            await _pedidoRepositorio.ActualizarAsync(pedido);
            return pedido;
        }

        private async Task AgregarItemsAPedidoAsync(Pedido pedido, List<ItemPedidoRequest> items)
        {
            if (items == null || items.Count == 0)
                throw new ReglaNegocioException("El pedido debe incluir al menos un producto.");

            foreach (var item in items)
            {
                var producto = await _productoRepositorio.ObtenerPorIdAsync(item.CodigoProducto);
                if (producto == null)
                    throw new EntidadNoEncontradaException($"El producto '{item.CodigoProducto}' no existe.");

                pedido.AgregarProducto(producto, item.Cantidad);
            }
        }
    }
}
