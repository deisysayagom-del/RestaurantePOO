using Microsoft.EntityFrameworkCore;
using RestaurantePOO.Core.Entities;
using RestaurantePOO.Core.Entities.Pedidos;
using RestaurantePOO.Core.Entities.Productos;

namespace RestaurantePOO.Infrastructure.Persistence
{
    public class RestauranteDbContext : DbContext
    {
        public RestauranteDbContext(DbContextOptions<RestauranteDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Estacion> Estaciones { get; set; }
        public DbSet<Repartidor> Repartidores { get; set; }
        public DbSet<ZonaReparto> ZonasReparto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. CLAVES PRIMARIAS (Se definen para que SQL Server sepa identificarlas)
            modelBuilder.Entity<Pedido>()
                .HasKey(p => p.Numero);
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Numero)
                .ValueGeneratedNever(); // Número manual para pedidos

            modelBuilder.Entity<Mesa>()
                .HasKey(m => m.Numero);
            modelBuilder.Entity<Mesa>()
                .Property(m => m.Numero)
                .ValueGeneratedNever(); // Número manual para mesas

            modelBuilder.Entity<Producto>()
                .HasKey(p => p.Codigo);

            // Claves primarias para las entidades de soporte
            modelBuilder.Entity<Estacion>()
                .HasKey(e => e.Nombre);

            modelBuilder.Entity<Insumo>()
                .HasKey(i => i.Nombre);

            modelBuilder.Entity<Repartidor>()
                .HasKey(r => r.Nombre);

            modelBuilder.Entity<ZonaReparto>()
                .HasKey(z => z.Nombre);

            // 2. MAPEO DE JERARQUÍA (TPH: Table Per Hierarchy)
            modelBuilder.Entity<Producto>()
                .HasDiscriminator<string>("TipoProducto")
                .HasValue<Plato>("Plato")
                .HasValue<Bebida>("Bebida")
                .HasValue<Combo>("Combo");

            modelBuilder.Entity<Pedido>()
                .HasDiscriminator<string>("TipoPedido")
                .HasValue<PedidoMesa>("Mesa")
                .HasValue<PedidoParaLlevar>("ParaLlevar")
                .HasValue<PedidoDomicilio>("Domicilio");

            // 3. CONFIGURACIÓN DE LLAVE COMPUESTA
            modelBuilder.Entity<DetallePedido>()
                .HasKey(dp => new { dp.PedidoNumero, dp.ProductoCodigo });
        }
    }
}
