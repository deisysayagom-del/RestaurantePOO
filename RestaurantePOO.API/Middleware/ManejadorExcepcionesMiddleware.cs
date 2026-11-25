using Microsoft.AspNetCore.Http;              // HttpContext (la petición y la respuesta) y RequestDelegate (el siguiente paso del pipeline)
using RestaurantePOO.Core.Exceptions;         // Las excepciones propias: EntidadNoEncontradaException y ReglaNegocioException
using System;                                 // Exception, ArgumentException
using System.Net;                             // HttpStatusCode (NotFound, BadRequest...)
using System.Text.Json;                       // JsonSerializer, para convertir el objeto de error a JSON
using System.Threading.Tasks;                 // Task, necesario para async/await

namespace RestaurantePOO.API.Middleware        // Capa API: el middleware no es del dominio, es parte de la infraestructura web
{
    public class ManejadorExcepcionesMiddleware // MIDDLEWARE: componente que envuelve TODAS las peticiones; aquí, para capturar errores en un solo lugar
    {
        private readonly RequestDelegate _next; // Referencia al SIGUIENTE paso del pipeline (otro middleware o, al final, el controlador); ENCAPSULAMIENTO: privado y solo lectura

        public ManejadorExcepcionesMiddleware(RequestDelegate next) // ASP.NET inyecta aquí el siguiente paso (INYECCIÓN DE DEPENDENCIAS)
        {
            _next = next;   // Lo guarda para poder llamarlo despues
        }

        public async Task InvokeAsync(HttpContext context) // Método que ASP.NET ejecuta en CADA petición (el nombre InvokeAsync es obligatorio por convención)
        {
            try                                  // Intenta ejecutar el resto del pipeline
            {
                await _next(context);            // Pasa la petición al siguiente paso (finalmente, el controlador); si todo sale bien, la respuesta ya va armada
            }
            catch (Exception ex)                 // Si ALGO lanza una excepción en cualquier punto (controlador, dominio, repositorio)
            {
                await ManejarExcepcionAsync(context, ex); //la captura aquí y arma una respuesta de error ordenada
            }
        }

        private static Task ManejarExcepcionAsync(HttpContext context, Exception excepcion) // Método auxiliar privado: traduce una excepción a una respuesta HTTP
        {
            context.Response.ContentType = "application/json"; // Indica que la respuesta será JSON


            var (codigoEstado, mensaje) = excepcion switch // "switch expression" con pattern matching: elige según el TIPO de la excepción; devuelve una tupla (código, mensaje)
            {
                EntidadNoEncontradaException ex => (HttpStatusCode.NotFound, ex.Message),  // No existe el recurso -> 404, con el mensaje del dominio
                ReglaNegocioException ex => (HttpStatusCode.BadRequest, ex.Message),       // Violó una regla del negocio -> 400, con su mensaje
                ArgumentException ex => (HttpStatusCode.BadRequest, ex.Message),           // Dato inválido en un constructor (también cubre ArgumentNullException, que hereda de ArgumentException) -> 400
                _ => (HttpStatusCode.InternalServerError, "Ha ocurrido un error interno en el servidor.") // "_" = cualquier otro tipo -> 500 con mensaje genérico (no expone detalles internos)
            };                   // El orden importa: se evalúa de arriba hacia abajo y gana la primera coincidencia


            context.Response.StatusCode = (int)codigoEstado;     // Asigna el código HTTP numérico a la respuesta (404, 400 o 500)

            var resultado = JsonSerializer.Serialize(new         // Convierte un objeto anónimo en un texto JSON
            {
                error = mensaje,                                 // Campo "error": el mensaje que verá el cliente
                status = (int)codigoEstado                       // Campo "status": el código numérico, repetido en el cuerpo para comodidad
            });

            return context.Response.WriteAsync(resultado);       // Escribe el JSON en el cuerpo de la respuesta y devuelve la Task para que se espere
        }
    }
}
