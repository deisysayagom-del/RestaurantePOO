using System;
using System.Collections.Generic;

namespace RestaurantePOO.API.DTOs
{
    public class ItemPedidoRequest
    {
        public string CodigoProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class CrearPedidoMesaRequest
    {
        public int NumeroPedido { get; set; }
        public int NumeroMesa { get; set; }
        public int CapacidadMesa { get; set; } = 4;
        public List<ItemPedidoRequest> Items { get; set; } = new List<ItemPedidoRequest>();
    }

    public class CrearPedidoParaLlevarRequest
    {
        public int NumeroPedido { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public bool EmpaqueEspecial { get; set; }
        public DateTime? HoraEstimadaRecogida { get; set; }
        public List<ItemPedidoRequest> Items { get; set; } = new List<ItemPedidoRequest>();
    }

    public class CrearPedidoDomicilioRequest
    {
        public int NumeroPedido { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string NombreRepartidor { get; set; } = string.Empty;
        public string TelefonoRepartidor { get; set; } = string.Empty;
        public string NombreZona { get; set; } = string.Empty;
        public decimal CostoEnvio { get; set; }
        public int TiempoEstimadoMinutos { get; set; } = 30;
        public List<ItemPedidoRequest> Items { get; set; } = new List<ItemPedidoRequest>();
    }
}
