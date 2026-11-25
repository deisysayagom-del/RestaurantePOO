using System;
using System.Linq;

namespace RestaurantePOO.Core.Entities.Pedidos
{
    public class PedidoParaLlevar : Pedido
    {
        public string NombreCliente { get; private set; }
        public bool EmpaqueEspecial { get; private set; }
        public DateTime HoraEstimadaRecogida { get; private set; }

        private PedidoParaLlevar() : base() { }

        public PedidoParaLlevar(int numero, string nombreCliente, bool empaqueEspecial, DateTime? horaEstimadaRecogida = null)
            : base(numero)
        {
            if (string.IsNullOrWhiteSpace(nombreCliente))
                throw new ArgumentException("El nombre del cliente es obligatorio.");

            NombreCliente = nombreCliente;
            EmpaqueEspecial = empaqueEspecial;
            HoraEstimadaRecogida = horaEstimadaRecogida ?? DateTime.Now.AddMinutes(20);
        }

        // Cobra un recargo adicional si el cliente solicita empaque termico/especial
        public override decimal Recargo()
        {
            return EmpaqueEspecial ? 1500m : 0m;
        }

        public override int TiempoEstimado()
        {
            int tiempoBase = Detalles.Sum(d => d.Producto.MinutosPreparacion() * d.Cantidad);
            return Math.Max(10, tiempoBase) + (EmpaqueEspecial ? 5 : 0);
        }
    }
}
