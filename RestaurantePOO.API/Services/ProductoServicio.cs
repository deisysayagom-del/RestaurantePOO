using RestaurantePOO.API.DTOs;
using RestaurantePOO.Core.Entities;
using RestaurantePOO.Core.Entities.Productos;
using RestaurantePOO.Core.Exceptions;
using RestaurantePOO.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantePOO.API.Services
{
    public interface IProductoServicio
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto> ObtenerPorCodigoAsync(string codigo);
        Task<IEnumerable<Producto>> ObtenerDisponiblesAsync();
        Task<Bebida> CrearBebidaAsync(CrearBebidaRequest request);
        Task<Plato> CrearPlatoAsync(CrearPlatoRequest request);
        Task EliminarAsync(string codigo);
    }

    public class ProductoServicio : IProductoServicio
    {
        private readonly IProductoRepositorio _productoRepositorio;

        public ProductoServicio(IProductoRepositorio productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _productoRepositorio.ObtenerTodosAsync();
        }

        public async Task<Producto> ObtenerPorCodigoAsync(string codigo)
        {
            var producto = await _productoRepositorio.ObtenerPorIdAsync(codigo);
            if (producto == null)
                throw new EntidadNoEncontradaException($"No se encontró ningún producto con el código '{codigo}'.");

            return producto;
        }

        public async Task<IEnumerable<Producto>> ObtenerDisponiblesAsync()
        {
            return await _productoRepositorio.ObtenerDisponiblesAsync();
        }

        public async Task<Bebida> CrearBebidaAsync(CrearBebidaRequest request)
        {
            var insumo = new Insumo(request.NombreInsumo, request.StockInicial, request.CostoUnidad, request.FechaVencimiento);
            var bebida = new Bebida(request.Codigo, request.Nombre, request.Precio, insumo, request.CantidadRequerida);

            await _productoRepositorio.AgregarAsync(bebida);
            return bebida;
        }

        public async Task<Plato> CrearPlatoAsync(CrearPlatoRequest request)
        {
            var estacion = new Estacion(request.NombreEstacion, request.CapacidadEstacion);
            var receta = request.Ingredientes.Select(i =>
            {
                var insumo = new Insumo(i.NombreInsumo, i.StockInicial, i.CostoUnidad);
                return new Ingrediente(i.Cantidad, insumo);
            }).ToList();

            var plato = new Plato(request.Codigo, request.Nombre, request.Precio, estacion, receta);

            await _productoRepositorio.AgregarAsync(plato);
            return plato;
        }

        public async Task EliminarAsync(string codigo)
        {
            var producto = await _productoRepositorio.ObtenerPorIdAsync(codigo);
            if (producto == null)
                throw new EntidadNoEncontradaException($"No se puede eliminar. El producto '{codigo}' no existe.");

            await _productoRepositorio.EliminarAsync(codigo);
        }
    }
}
