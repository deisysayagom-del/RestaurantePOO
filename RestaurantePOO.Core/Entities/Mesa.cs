using System;
using RestaurantePOO.Core.Enums;

namespace RestaurantePOO.Core.Entities
{
    public class Mesa
    {
        public int Numero { get; private set; }
        public int Capacidad { get; private set; }
        public EstadoMesa Estado { get; private set; }

        public Mesa(int numero, int capacidad)
        {
            if (numero <= 0)
                throw new ArgumentException("El número de mesa debe ser mayor a cero.");

            if (capacidad <= 0)
                throw new ArgumentException("La capacidad debe ser mayor a cero.");

            Numero = numero;
            Capacidad = capacidad;
            Estado = EstadoMesa.LIBRE;
        }

        public void CambiarEstado(EstadoMesa nuevoEstado)
        {
            Estado = nuevoEstado;
        }
    }
}