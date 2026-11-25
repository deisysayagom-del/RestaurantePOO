using System;

namespace RestaurantePOO.Core.Entities
{
    public class ZonaReparto
    {
        public string Nombre { get; private set; }
        public decimal CostoEnvio { get; private set; }
        public int TiempoEstimadoMinutos { get; private set; }

        private ZonaReparto() { }

        public ZonaReparto(string nombre, decimal costoEnvio, int tiempoEstimadoMinutos = 30)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la zona es obligatorio.");

            if (costoEnvio < 0)
                throw new ArgumentException("El costo de envío no puede ser negativo.");

            Nombre = nombre;
            CostoEnvio = costoEnvio;
            TiempoEstimadoMinutos = tiempoEstimadoMinutos;
        }
    }
}
