using System;                                 // ArgumentException
using System.Collections.Generic;             // List<T>
using System.Linq;                            // Any(), Min(), Max()

namespace RestaurantePOO.Core.Entities.Productos   // Capa Core (dominio), carpeta de productos
{
    public class Combo : Producto             // HERENCIA: Combo "es un" Producto; hereda Codigo, Nombre, Precio, Activo...
    {
        public List<Producto> Componentes { get; private set; } = new List<Producto>();   // COMPOSICIÓN: el combo "tiene" productos. La lista es de tipo Producto,
                                                                                          // así que puede guardar Bebida, Plato o hasta otro Combo (POLIMORFISMO)

        // Constructor para Entity Framework Core
        protected Combo() : base("TEMP", "TEMP", 0) { }   // Constructor vacío para el ORM; los valores "TEMP" evitan que fallen las validaciones de Producto

        public Combo(string codigo, string nombre, decimal precio, List<Producto> componentes, bool activo = true)   // "activo = true": parámetro opcional
            : base(codigo, nombre, precio, activo)    // Delega en Producto la validación y asignación de los datos comunes
        {
            Componentes = componentes ?? new List<Producto>();   // "??": si llega null, usa una lista vacía para no tener un null

            if (!Componentes.Any())                   // VALIDACIÓN: un combo vacío no tiene sentido
                throw new ArgumentException("Un combo debe tener al menos un producto componente.");
        }

        public override int Disponibilidad()          // POLIMORFISMO: cada tipo de producto calcula su disponibilidad a su manera
        {
            if (!Activo || Componentes == null || !Componentes.Any()) return 0;   // Si está inactivo o no tiene componentes, no hay disponibilidad
            return Componentes.Min(p => p.Disponibilidad());   // Toma la MENOR disponibilidad de los componentes: el combo se limita por el que más se agota
        }                                                      // Cada p.Disponibilidad() se ejecuta según el tipo real (Bebida o Plato): POLIMORFISMO

        public override int MinutosPreparacion()      // POLIMORFISMO
        {
            if (Componentes == null || !Componentes.Any()) return 0;   // Sin componentes, no hay tiempo de preparación
            return Componentes.Max(p => p.MinutosPreparacion());       // Toma el MAYOR tiempo: se asume que los componentes se preparan en paralelo
        }

        public override void DescontarInventario(int cantidad)   // POLIMORFISMO: descuenta del inventario
        {
            if (cantidad <= 0)                        // Valida que la cantidad sea positiva
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            foreach (var componente in Componentes)   // Recorre cada producto del combo
            {
                componente.DescontarInventario(cantidad);   // Le pide a cada uno que descuente su propio inventario; el combo no sabe cómo (Bebida y Plato lo hacen distinto)
            }
        }
    }
}