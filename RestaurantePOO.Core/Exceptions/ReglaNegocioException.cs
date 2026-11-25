using System;                                 // Trae la clase base Exception

namespace RestaurantePOO.Core.Exceptions      // Carpeta de excepciones propias del dominio (Core), junto a EntidadNoEncontradaException
{
    public class ReglaNegocioException : Exception   // HERENCIA: hereda de Exception, así que se puede lanzar con "throw" y capturar con "catch"
    {
        public ReglaNegocioException(string message) : base(message) { }   // Recibe el mensaje y se lo pasa al constructor de Exception con ": base(message)"
    }                                                                      // El cuerpo { } está vacío: el mensaje queda guardado en la clase base
}