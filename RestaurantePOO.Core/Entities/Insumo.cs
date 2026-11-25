using System;                                 // DateTime, ArgumentException, InvalidOperationException

namespace RestaurantePOO.Core.Entities        // Capa Core (dominio), entidades generales
{
    public class Insumo                       // Materia prima del inventario (carne, pan, gaseosa...). Es quien REALMENTE guarda el stock
    {
        public string Nombre { get; private set; }            // Nombre del insumo; private set: solo se modifica dentro de la clase (ENCAPSULAMIENTO)
        public decimal Existencia { get; private set; }       // Stock actual; NADIE de afuera puede hacer insumo.Existencia = -50, solo Descontar() y Reabastecer()
        public decimal CostoUnidad { get; private set; }      // Costo por unidad del insumo
        public DateTime? FechaVencimiento { get; private set; }   // El "?" lo hace nullable: es opcional (no todo insumo vence)

        // Constructor para Entity Framework Core
        private Insumo() { }                  // Constructor vacío y PRIVADO: lo usa el ORM para reconstruir el objeto desde la BD

        public Insumo(string nombre, decimal existencia, decimal costoUnidad = 0m, DateTime? fechaVencimiento = null)   // Parámetros opcionales: costo 0 y sin vencimiento por defecto
        {
            if (string.IsNullOrWhiteSpace(nombre))        // VALIDACIÓN: el nombre es obligatorio
                throw new ArgumentException("El nombre del insumo es obligatorio.");

            if (existencia < 0)                           // VALIDACIÓN: no puede haber stock negativo (el 0 sí se permite)
                throw new ArgumentException("La existencia no puede ser negativa.");

            if (costoUnidad < 0)                          // VALIDACIÓN: no existen costos negativos
                throw new ArgumentException("El costo por unidad no puede ser negativo.");

            Nombre = nombre;                              // Si todo es válido, asigna los datos: el objeto nunca nace en un estado inválido
            Existencia = existencia;
            CostoUnidad = costoUnidad;
            FechaVencimiento = fechaVencimiento;
        }

        public bool EstaAgotado()             // Pregunta simple que otras clases usan (Bebida, Ingrediente) sin acceder a los datos internos
        {
            return Existencia <= 0;           // true si ya no queda stock
        }

        public void Descontar(decimal cantidad)   // Único camino para RESTAR stock (lo llaman Bebida y Plato al vender)
        {
            if (cantidad <= 0)                // VALIDACIÓN: solo se descuenta una cantidad positiva
                throw new ArgumentException("La cantidad a descontar debe ser mayor a cero.");

            if (cantidad > Existencia)        // REGLA DE NEGOCIO: no se puede vender más de lo que hay
                throw new InvalidOperationException($"Stock insuficiente para el insumo '{Nombre}'. Disponible: {Existencia}.");

            Existencia -= cantidad;           // Si hay suficiente, resta. El propio objeto protege su estado (ENCAPSULAMIENTO)
        }

        public void Reabastecer(decimal cantidad)   // Único camino para SUMAR stock (cuando llega mercancía)
        {
            if (cantidad <= 0)                // VALIDACIÓN: solo se reabastece una cantidad positiva
                throw new ArgumentException("La cantidad a reabastecer debe ser mayor a cero.");

            Existencia += cantidad;           // Suma al stock actual
        }
    }
}
