using RestaurantePOO.Core.Entities.Productos;
using System;

namespace RestaurantePOO.Core.Entities.Productos
{
    public class DetallePedido
    {
        public int PedidoNumero { get; private set; }
        public string ProductoCodigo { get; private set; } = string.Empty;
        public Producto Producto { get; private set; } = null!;
        public int Cantidad { get; private set; }
        public decimal PrecioUnitario { get; private set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Constructor vacio para Entity Framework Core
        private DetallePedido() { }

        public DetallePedido(int pedidoNumero, Producto producto, int cantidad)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            PedidoNumero = pedidoNumero;
            Producto = producto;
            ProductoCodigo = producto.Codigo;
            Cantidad = cantidad;
            PrecioUnitario = producto.Precio;
        }

        public void AumentarCantidad(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad a aumentar debe ser mayor a cero.");

            Cantidad += cantidad;
        }
    }
}

