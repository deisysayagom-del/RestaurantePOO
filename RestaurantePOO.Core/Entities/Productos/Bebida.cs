using System;                                 // ArgumentNullException, ArgumentException

namespace RestaurantePOO.Core.Entities.Productos   // Capa Core (dominio), carpeta de productos
{
    public class Bebida : Producto            // HERENCIA: Bebida "es un" Producto; hereda Codigo, Nombre, Precio, Activo...
    {
        public Insumo InsumoDirecto { get; private set; }          // COMPOSICIÓN: la bebida "tiene un" Insumo que consume directamente (ej. gaseosa, jugo)
        public decimal CantidadRequerida { get; private set; }     // Cuánto insumo gasta cada bebida; private set (ENCAPSULAMIENTO)

        // Constructor para Entity Framework Core
        protected Bebida() : base("TEMP", "TEMP", 0) { }   // Constructor vacío para el ORM; protected para que solo hijas/EF lo usen.
                                                           // Pasa valores "TEMP" al constructor de Producto para que sus validaciones no fallen; EF luego sobrescribe los datos reales

        public Bebida(string codigo, string nombre, decimal precio, Insumo insumoDirecto, decimal cantidadRequerida, bool activo = true)   // "activo = true": parámetro opcional
            : base(codigo, nombre, precio, activo)    // Delega en Producto la validación y asignación de los datos comunes
        {
            InsumoDirecto = insumoDirecto ?? throw new ArgumentNullException(nameof(insumoDirecto));   // Una bebida no puede existir sin insumo: si es null, lanza excepción

            if (cantidadRequerida <= 0)               // VALIDACIÓN: no tendría sentido gastar 0 o menos insumo
                throw new ArgumentException("La cantidad requerida debe ser mayor a cero.");

            CantidadRequerida = cantidadRequerida;    // Si es válida, la asigna
        }

        public override int Disponibilidad()          // POLIMORFISMO: implementa/reemplaza el método de Producto; cada tipo calcula su disponibilidad a su manera
        {
            if (!Activo || InsumoDirecto == null || InsumoDirecto.EstaAgotado() || CantidadRequerida <= 0)   // Si está inactiva, sin insumo, con insumo agotado o dato inválido...
                return 0;                             // ...no hay unidades disponibles

            return (int)(InsumoDirecto.Existencia / CantidadRequerida);   // Existencia / cantidad por bebida = cuántas bebidas se pueden preparar; (int) descarta decimales
        }

        public override int MinutosPreparacion()      // POLIMORFISMO: cada producto define su propio tiempo de preparación
        {
            return 3;                                 // Una bebida tarda 3 minutos (un Plato tendrá otro valor)
        }

        public override void DescontarInventario(int cantidad)   // POLIMORFISMO: descuenta del inventario según el tipo de producto
        {
            if (cantidad <= 0)                        // Valida que se descuente una cantidad positiva
                throw new ArgumentException("La cantidad a descontar debe ser mayor a cero.");

            InsumoDirecto.Descontar(CantidadRequerida * cantidad);   // Le pide al Insumo que se descuente: insumo por bebida × número de bebidas vendidas (el Insumo gestiona su propio stock)
        }
    }
}