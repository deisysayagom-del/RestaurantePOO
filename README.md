# ComandaPro (RestaurantePOO)

Aplicación web y API REST desarrollada en C# (.NET 10) para administrar el catálogo de productos, la toma de pedidos (en mesa, para llevar y a domicilio) y el flujo de estados en cocina con descuento automático de inventario en SQL Server.

## Integrante

* Deisy Alexandra Sayago Mestra

## Estructura del proyecto

La solución está organizada en tres capas principales:

* **RestaurantePOO.Core:** Entidades del dominio (`Producto`, `Bebida`, `Plato`, `Combo`, `Pedido`, `PedidoMesa`, `PedidoParaLlevar`, `PedidoDomicilio`, `Insumo`), reglas de negocio, manejo de estados, excepciones e interfaces de repositorio.
* **RestaurantePOO.Infrastructure:** Configuración de Entity Framework Core (`RestauranteDbContext`) con mapeo de herencia TPH y repositorios conectados a SQL Server LocalDB.
* **RestaurantePOO.API:** Controladores REST, servicios de aplicación, DTOs, middleware global de excepciones e interfaz web estática ubicada en `wwwroot`.

## Requisitos para ejecutarlo

* .NET 10 SDK
* SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)

## Cómo ejecutar el proyecto

1. Clonar el repositorio:
   ```bash
   git clone [https://github.com/deisysayagom-del/RestaurantePOO.git](https://github.com/deisysayagom-del/RestaurantePOO.git)
   ```
2. Abrir `RestaurantePOO.slnx` en Visual Studio 2022/2026 y presionar **F5** (o ejecutar desde terminal con `dotnet run --project RestaurantePOO.API --launch-profile https`).
3. Al iniciar, el sistema crea automáticamente la base de datos `RestauranteDB` en LocalDB si aún no existe.

## Rutas de acceso

* **Menú y Productos:** `https://localhost:7051/index.html`
* **Toma de Pedidos:** `https://localhost:7051/pedidos.html`
* **Monitor de Cocina:** `https://localhost:7051/cocina.html`
* **Swagger (pruebas de endpoints):** `https://localhost:7051/swagger`

## Declaración de uso de IA

1. Se utilizó inteligencia artificial como apoyo para estructurar los servicios, DTOs en .NET 10 y la maquetación accesible de las vistas en `wwwroot`.
2. El código generado fue revisado, ajustado manualmente a las reglas del dominio (polimorfismo en pedidos, encapsulamiento de estados y descuento de stock) y depurado en Visual Studio.
3. Todo el flujo fue probado de extremo a extremo desde el navegador hasta la persistencia en SQL Server LocalDB.