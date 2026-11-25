using Microsoft.AspNetCore.Mvc;
using RestaurantePOO.API.DTOs;
using RestaurantePOO.API.Services;
using RestaurantePOO.Core.Entities.Productos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantePOO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoServicio _productoServicio;

        public ProductosController(IProductoServicio productoServicio)
        {
            _productoServicio = productoServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerTodos()
        {
            var productos = await _productoServicio.ObtenerTodosAsync();
            return Ok(productos);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult<Producto>> ObtenerPorCodigo(string codigo)
        {
            var producto = await _productoServicio.ObtenerPorCodigoAsync(codigo);
            return Ok(producto);
        }

        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerDisponibles()
        {
            var disponibles = await _productoServicio.ObtenerDisponiblesAsync();
            return Ok(disponibles);
        }

        [HttpPost("bebida")]
        public async Task<IActionResult> CrearBebida([FromBody] CrearBebidaRequest request)
        {
            var bebida = await _productoServicio.CrearBebidaAsync(request);
            return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = bebida.Codigo }, bebida);
        }

        [HttpPost("plato")]
        public async Task<IActionResult> CrearPlato([FromBody] CrearPlatoRequest request)
        {
            var plato = await _productoServicio.CrearPlatoAsync(request);
            return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = plato.Codigo }, plato);
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Eliminar(string codigo)
        {
            await _productoServicio.EliminarAsync(codigo);
            return NoContent();
        }
    }
}
