using System;

namespace RestaurantePOO.Core.Entities
{
    public class Repartidor
    {
        public string Nombre { get; private set; }
        public string Telefono { get; private set; }
        public bool Disponible { get; private set; }

        // Constructor para Entity Framework Core
        private Repartidor() { }

        public Repartidor(string nombre, string telefono, bool disponible = true)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del repartidor es obligatorio.");

            Nombre = nombre;
            Telefono = telefono;
            Disponible = disponible;
        }

        public void CambiarDisponibilidad(bool disponible)
        {
            Disponible = disponible;
        }
    }
}
