using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantePOO.Core.Entities
{
    public class Estacion
    {
        [Key] // Clave primaria requerida por SQL Server
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public int TiempoEstimadoMinutos { get; private set; } = 15;
        public bool Activa { get; private set; }

        // Constructor privado para Entity Framework Core
        private Estacion() { }

        // Constructor 1: Acepta int como segundo parámetro (resuelve el error CS1503)
        public Estacion(string nombre, int tiempoEstimadoMinutos)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la estación es obligatorio.", nameof(nombre));

            Nombre = nombre;
            TiempoEstimadoMinutos = tiempoEstimadoMinutos > 0 ? tiempoEstimadoMinutos : 15;
            Descripcion = $"Estación de {nombre}";
            Activa = true;
        }

        // Constructor 2: Acepta string como segundo parámetro
        public Estacion(string nombre, string descripcion = "")
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la estación es obligatorio.", nameof(nombre));

            Nombre = nombre;
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? $"Estación de {nombre}" : descripcion;
            TiempoEstimadoMinutos = 15;
            Activa = true;
        }

        // Constructor 3: Acepta tanto tiempo como descripción
        public Estacion(string nombre, int tiempoEstimadoMinutos, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la estación es obligatorio.", nameof(nombre));

            Nombre = nombre;
            TiempoEstimadoMinutos = tiempoEstimadoMinutos;
            Descripcion = descripcion;
            Activa = true;
        }

        public void Desactivar()
        {
            Activa = false;
        }

        public void Activar()
        {
            Activa = true;
        }
    }
}
