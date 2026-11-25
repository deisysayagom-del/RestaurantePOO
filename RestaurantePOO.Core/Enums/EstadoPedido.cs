using System.Text.Json.Serialization; //  Namespace necesario para la conversión

namespace RestaurantePOO.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // Fuerza a que se vea como texto en el JSON
    public enum EstadoPedido
    {
        Registrado,
        Confirmado,
        EnPreparacion,
        Listo,
        Entregado
    }
}

