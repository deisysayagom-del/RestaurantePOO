using Microsoft.AspNetCore.Mvc;               // Clases para crear controladores web: ControllerBase, [HttpGet], Ok()
using RestaurantePOO.Core.Entities;           // Entidades del dominio: Insumo, Ingrediente, Estacion, Mesa
using RestaurantePOO.Core.Entities.Pedidos;   // Tipos de pedido: PedidoMesa
using RestaurantePOO.Core.Entities.Productos; // Tipos de producto: Plato
using System.Collections.Generic;             // List<T>

namespace RestaurantePOO.API.Controllers      // Agrupa la clase dentro de la capa API, carpeta Controllers
{
    [ApiController]                           // Activa comportamientos automáticos de API
    [Route("api/[controller]")]               // Ruta base: [controller] -> "PruebaDomain" -> api/pruebadomain
    public class PruebaDomainController : ControllerBase   // HERENCIA: hereda de ControllerBase para usar Ok()
    {
        [HttpGet("test-dominio")]             // Responde a GET api/pruebadomain/test-dominio
        public IActionResult TestDominio()    // Método síncrono: no usa base de datos, todo se hace en memoria
        {
            var insumo1 = new Insumo("Carne de Res", 100, 2500);          // Crea un insumo: nombre, stock (100) y costo por unidad (2500)
            var insumo2 = new Insumo("Pan de Hamburguesa", 50, 800);      // Otro insumo: stock 50 y costo 800

            var receta = new List<Ingrediente>                            // La receta es una lista de objetos Ingrediente
            {
                new Ingrediente(1, insumo1),                              // Ingrediente: usa 1 unidad de carne (COMPOSICIÓN: Ingrediente "tiene un" Insumo)
                new Ingrediente(1, insumo2)                               // Ingrediente: usa 1 unidad de pan
            };

            var estacion = new Estacion("Parrilla", 5);                   // Estación de cocina: nombre y capacidad (5)
            var plato = new Plato("PLT-001", "Hamburguesa Clasica", 18000, estacion, receta);   // Plato: código, nombre, precio, estación y receta (HEREDA de Producto)

            var mesa = new Mesa(1, 4);                                    // Mesa número 1 con capacidad para 4 personas
            var pedidoMesa = new PedidoMesa(101, mesa);                   // Pedido número 101 asociado a la mesa (PedidoMesa HEREDA de Pedido)
            pedidoMesa.AgregarProducto(plato, 2);                         // Agrega 2 hamburguesas; el pedido valida y guarda el producto (ENCAPSULAMIENTO)

            return Ok(new                                                 // Responde 200 OK con un objeto anónimo convertido a JSON
            {
                Mensaje = "Prueba de dominio ejecutada con éxito",        // Texto de confirmación
                PedidoNumero = pedidoMesa.Numero,                         // Número del pedido (101)
                NumeroMesa = pedidoMesa.Mesa.Numero,                      // Navega del pedido a su mesa y toma su número (1)
                TotalCalculado = pedidoMesa.CalcularTotal(),              // El pedido calcula su propio total (el controlador no hace la cuenta)
                TiempoEstimado = pedidoMesa.TiempoEstimado()              // El pedido calcula su tiempo estimado (puede variar según el tipo de pedido: POLIMORFISMO)
            });
        }
    }
}