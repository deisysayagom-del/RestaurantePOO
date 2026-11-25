using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantePOO.Core.Entities.Productos
{
    public class Plato : Producto
    {
        public List<Ingrediente> Receta { get; private set; } = new List<Ingrediente>();
        public Estacion Estacion { get; private set; }

        // Constructor para Entity Framework Core
        protected Plato() : base("TEMP", "TEMP", 0) { }

        public Plato(string codigo, string nombre, decimal precio, Estacion estacion, List<Ingrediente> receta, bool activo = true)
            : base(codigo, nombre, precio, activo)
        {
            Estacion = estacion ?? throw new ArgumentNullException(nameof(estacion));
            Receta = receta ?? new List<Ingrediente>();

            if (!Receta.Any())
                throw new ArgumentException("Un plato debe tener al menos un ingrediente en su receta.");
        }

        public override int Disponibilidad()
        {
            if (!Activo || Receta == null || !Receta.Any()) return 0;
            return Receta.Min(i => i.AlcanzaPara());
        }

        public override int MinutosPreparacion()
        {
            return 15;
        }

        public override void DescontarInventario(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad a descontar debe ser mayor a cero.");

            foreach (var ingrediente in Receta)
            {
                ingrediente.Insumo.Descontar(ingrediente.Cantidad * cantidad);
            }
        }
    }
}
