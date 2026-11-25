using Microsoft.AspNetCore.Mvc;
using RestaurantePOO.API.DTOs;
using RestaurantePOO.API.Services;
using RestaurantePOO.Core.Entities.Pedidos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantePOO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoServicio _pedidoServicio;

        public PedidosController(IPedidoServicio pedidoServicio)
        {
            _pedidoServicio = pedidoServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> ObtenerTodos()
        {
            var pedidos = await _pedidoServicio.ObtenerTodosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{numero}")]
        public async Task<ActionResult<Pedido>> ObtenerPorNumero(int numero)
        {
            var pedido = await _pedidoServicio.ObtenerPorNumeroAsync(numero);
            return Ok(pedido);
        }

        [HttpPost("mesa")]
        public async Task<IActionResult> CrearPedidoMesa([FromBody] CrearPedidoMesaRequest request)
        {
            var pedido = await _pedidoServicio.CrearPedidoMesaAsync(request);
            return CreatedAtAction(nameof(ObtenerPorNumero), new { numero = pedido.Numero }, pedido);
        }

        [HttpPost("para-llevar")]
        public async Task<IActionResult> CrearPedidoParaLlevar([FromBody] CrearPedidoParaLlevarRequest request)
        {
            var pedido = await _pedidoServicio.CrearPedidoParaLlevarAsync(request);
            return CreatedAtAction(nameof(ObtenerPorNumero), new { numero = pedido.Numero }, pedido);
        }

        [HttpPost("domicilio")]
        public async Task<IActionResult> CrearPedidoDomicilio([FromBody] CrearPedidoDomicilioRequest request)
        {
            var pedido = await _pedidoServicio.CrearPedidoDomicilioAsync(request);
            return CreatedAtAction(nameof(ObtenerPorNumero), new { numero = pedido.Numero }, pedido);
        }

        [HttpPut("{numero}/avanzar-estado")]
        public async Task<IActionResult> AvanzarEstado(int numero)
        {
            var pedido = await _pedidoServicio.AvanzarEstadoAsync(numero);
            return Ok(pedido);
        }
    }
}
