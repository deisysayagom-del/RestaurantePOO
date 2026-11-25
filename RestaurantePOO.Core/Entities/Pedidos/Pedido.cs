using RestaurantePOO.Core.Entities.Productos;
using RestaurantePOO.Core.Enums;
using RestaurantePOO.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantePOO.Core.Entities.Pedidos
{
    // Clase padre abstracta: No se pueden crear pedidos genericos sin especificar su tipo.
    public abstract class Pedido
    {
        public int Numero { get; protected set; }
        public DateTime FechaHora { get; protected set; }
        public EstadoPedido Estado { get; protected set; }
        public List<DetallePedido> Detalles { get; protected set; } = new List<DetallePedido>();

        protected Pedido()
        {
            FechaHora = DateTime.Now;
            Estado = EstadoPedido.Registrado;
        }

        protected Pedido(int numero) : this()
        {
            if (numero <= 0)
                throw new ArgumentException("El número de pedido debe ser mayor a cero.");

            Numero = numero;
        }

        // Metodos abstractos obligan a que cada subclase defina su regla de cobro y tiempo
        public abstract decimal Recargo();
        public abstract int TiempoEstimado();

        public virtual void AgregarProducto(Producto producto, int cantidad)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            var detalleExistente = Detalles.FirstOrDefault(d => d.ProductoCodigo == producto.Codigo);
            if (detalleExistente != null)
            {
                detalleExistente.AumentarCantidad(cantidad);
            }
            else
            {
                Detalles.Add(new DetallePedido(Numero, producto, cantidad));
            }
        }

        public virtual decimal CalcularTotal()
        {
            return Detalles.Sum(d => d.Subtotal) + Recargo();
        }

        // Maquina de estados encapsulada dentro de la entidad 
        public void AvanzarEstado()
        {
            switch (Estado)
            {
                case EstadoPedido.Registrado:
                    Confirmar();
                    break;
                case EstadoPedido.Confirmado:
                    MarcarEnPreparacion();
                    break;
                case EstadoPedido.EnPreparacion:
                    MarcarListo();
                    break;
                case EstadoPedido.Listo:
                    Entregar();
                    break;
                default:
                    throw new ReglaNegocioException($"El pedido #{Numero} ya está en estado final ({Estado}).");
            }
        }

        public void Confirmar()
        {
            if (Estado != EstadoPedido.Registrado)
                throw new ReglaNegocioException($"No se puede confirmar un pedido en estado '{Estado}'.");

            if (!Detalles.Any())
                throw new ReglaNegocioException("No se puede confirmar un pedido sin productos.");

            // Descuenta el inventario polimorficamente por cada producto del pedido
            foreach (var detalle in Detalles)
            {
                if (detalle.Producto != null)
                {
                    detalle.Producto.DescontarInventario(detalle.Cantidad);
                }
            }

            Estado = EstadoPedido.Confirmado;
        }

        public void MarcarEnPreparacion()
        {
            if (Estado != EstadoPedido.Confirmado)
                throw new ReglaNegocioException($"No se puede enviar a preparación un pedido en estado '{Estado}'.");

            Estado = EstadoPedido.EnPreparacion;
        }

        public void MarcarListo()
        {
            if (Estado != EstadoPedido.EnPreparacion)
                throw new ReglaNegocioException($"No se puede marcar listo un pedido en estado '{Estado}'.");

            Estado = EstadoPedido.Listo;
        }

        public void Entregar()
        {
            if (Estado != EstadoPedido.Listo)
                throw new ReglaNegocioException($"No se puede entregar un pedido en estado '{Estado}'.");

            Estado = EstadoPedido.Entregado;
        }
    }
}
