using System;
using System.Linq;

namespace RestaurantePOO.Core.Entities.Pedidos
{
    public class PedidoDomicilio : Pedido
    {
        public string Direccion { get; private set; } = string.Empty;
        public Repartidor Repartidor { get; private set; }
        public ZonaReparto Zona { get; private set; }

        private PedidoDomicilio() : base() { }

        public PedidoDomicilio(int numero, string direccion, Repartidor repartidor, ZonaReparto zona)
            : base(numero)
        {
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ArgumentException("La dirección de entrega es obligatoria.");

            Direccion = direccion;
            Repartidor = repartidor ?? throw new ArgumentNullException(nameof(repartidor));
            Zona = zona ?? throw new ArgumentNullException(nameof(zona));
        }

        public override decimal Recargo()
        {
            return Zona?.CostoEnvio ?? 0m;
        }

        public override int TiempoEstimado()
        {
            int tiempoProduccion = Detalles.Sum(d => d.Producto.MinutosPreparacion() * d.Cantidad);
            int tiempoEnvio = Zona?.TiempoEstimadoMinutos ?? 30;
            return tiempoProduccion + tiempoEnvio;
        }
    }
}
