using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantePOO.Core.Entities
{
    public class Ingrediente
    {
        [Key]
        public int Id { get; private set; }
        public Insumo Insumo { get; private set; } = null!;
        public decimal Cantidad { get; private set; }

        // Constructor privado requerido por Entity Framework Core
        private Ingrediente() { }

        public Ingrediente(decimal cantidad, Insumo insumo)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad del ingrediente debe ser mayor a cero.", nameof(cantidad));

            Insumo = insumo ?? throw new ArgumentNullException(nameof(insumo));
            Cantidad = cantidad;
        }

        public int AlcanzaPara()
        {
            if (Insumo == null || Insumo.EstaAgotado() || Cantidad <= 0)
                return 0;

            return (int)(Insumo.Existencia / Cantidad);
        }
    }
}
