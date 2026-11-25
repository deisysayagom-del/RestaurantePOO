using System;
using System.Collections.Generic;

namespace RestaurantePOO.API.DTOs
{
    public class CrearBebidaRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string NombreInsumo { get; set; } = string.Empty;
        public decimal StockInicial { get; set; }
        public decimal CostoUnidad { get; set; } = 0m;
        public DateTime? FechaVencimiento { get; set; }
        public decimal CantidadRequerida { get; set; } = 1;
    }

    public class CrearPlatoRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string NombreEstacion { get; set; } = string.Empty;
        public int CapacidadEstacion { get; set; } = 10;
        public List<IngredienteRequest> Ingredientes { get; set; } = new List<IngredienteRequest>();
    }

    public class IngredienteRequest
    {
        public string NombreInsumo { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal StockInicial { get; set; }
        public decimal CostoUnidad { get; set; } = 0m;
    }
}
