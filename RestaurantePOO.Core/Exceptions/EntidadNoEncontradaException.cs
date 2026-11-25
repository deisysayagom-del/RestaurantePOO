using System;                                 // Trae la clase base Exception

namespace RestaurantePOO.Core.Exceptions      // Carpeta de excepciones propias del dominio (Core)
{
    public class EntidadNoEncontradaException : Exception   // HERENCIA: hereda de Exception, así que se puede lanzar con "throw" y capturar con "catch"
    {
        public EntidadNoEncontradaException(string message) : base(message) { }   // Recibe el mensaje y se lo pasa al constructor de Exception con ": base(message)"
    }                                                                             // El cuerpo { } está vacío: no necesita hacer nada más, el mensaje queda guardado en la clase base
}
