using System;                                 // ArgumentException

namespace RestaurantePOO.Core.Entities.Productos   // Capa Core (dominio), carpeta de productos
{
    public abstract class Producto            // ABSTRACCIÓN: clase abstracta, NO se puede instanciar (new Producto() da error); es la base de Bebida, Plato y Combo
    {
        public string Codigo { get; protected set; }    // Identificador único; se lee desde afuera, pero solo esta clase y sus hijas pueden modificarlo (ENCAPSULAMIENTO)
        public string Nombre { get; protected set; }    // Nombre del producto
        public decimal Precio { get; protected set; }   // Precio de venta; decimal: precisión exacta para dinero
        public bool Activo { get; protected set; }      // Indica si el producto se puede vender

        protected Producto(string codigo, string nombre, decimal precio, bool activo = true)   // Constructor protected: solo las hijas pueden llamarlo; "activo = true": opcional
        {
            if (string.IsNullOrWhiteSpace(codigo))      // VALIDACIÓN: el código no puede ser null, vacío ni solo espacios
                throw new ArgumentException("El código del producto no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(nombre))      // VALIDACIÓN: el nombre es obligatorio
                throw new ArgumentException("El nombre del producto no puede estar vacío.");

            if (precio < 0)                             // VALIDACIÓN: no existen precios negativos (el 0 sí se permite)
                throw new ArgumentException("El precio no puede ser negativo.");

            Codigo = codigo;                            // Si todo es válido, asigna los datos: el objeto nunca nace en un estado inválido
            Nombre = nombre;
            Precio = precio;
            Activo = activo;
        }

        // Métodos abstractos (se implementan en cada clase hija)
        public abstract int Disponibilidad();           // Sin cuerpo: es un CONTRATO. Toda hija está OBLIGADA a implementarlo con "override" (cuántas unidades se pueden vender)
        public abstract int MinutosPreparacion();       // Contrato: cuánto tarda en prepararse (Bebida: 3; Combo: el mayor de sus componentes)
        public abstract void DescontarInventario(int cantidad);   // Contrato: cómo se descuenta el stock al vender; cada tipo lo hace distinto
    }
}