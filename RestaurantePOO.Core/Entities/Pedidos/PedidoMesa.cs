using System;
using System.Linq;

namespace RestaurantePOO.Core.Entities.Pedidos
{
    public class PedidoMesa : Pedido
    {
        public Mesa Mesa { get; private set; }

        private PedidoMesa() : base() { }

        public PedidoMesa(int numero, Mesa mesa) : base(numero)
        {
            Mesa = mesa ?? throw new ArgumentNullException(nameof(mesa));
        }

        public override decimal Recargo() => 0m;

        // El tiempo de preparacion se calcula segun los productos reales ingresados
        public override int TiempoEstimado()
        {
            int tiempoPreparacion = Detalles.Sum(d => d.Producto.MinutosPreparacion() * d.Cantidad);
            return Math.Max(15, tiempoPreparacion); // Minimo 15 minutos de atencion en mesa
        }
    }
}
