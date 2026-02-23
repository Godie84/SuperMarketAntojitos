# SuperMarket Antojitos
### Sistema de Gestión de Ventas

> Aplicativo web desarrollado en ASP.NET Core MVC .NET 6 para digitalizar y controlar las ventas diarias, el inventario y los clientes del Supermercado Antojitos, ubicado en el barrio Vivir Bueno.

---

## Tabla de Contenidos

- [Descripción del Proyecto](#descripción-del-proyecto)
- [Problema que Resuelve](#problema-que-resuelve)
- [Funcionalidades](#funcionalidades)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Arquitectura y Patrones](#arquitectura-y-patrones)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Modelo de Datos](#modelo-de-datos)
- [Requisitos Previos](#requisitos-previos)
- [Instalación y Configuración](#instalación-y-configuración)
- [Credenciales por Defecto](#credenciales-por-defecto)
- [Roles del Sistema](#roles-del-sistema)
- [Módulos del Sistema](#módulos-del-sistema)
- [Reportes](#reportes)
- [Autores](#autores)

---

## Descripción del Proyecto

El Supermercado Antojitos lleva más de 10 años en el mercado registrando sus ventas de forma manual, lo que genera descuadres de caja, pérdida de dinero y falta de control sobre el inventario. Este sistema digitaliza todo el proceso de ventas, permitiendo a la gerencia tener visibilidad en tiempo real sobre el stock de productos, las ventas del día y el historial de clientes.

---

## Problema que Resuelve

| Problema Anterior | Solución Implementada |
|---|---|
| Registro manual de ventas | Registro digital con trazabilidad completa |
| Descuadres de caja | Transacciones atómicas en base de datos |
| Sin control de inventario | Stock en tiempo real con validación automática |
| Sin historial de clientes | Registro obligatorio de cliente por venta |
| Sin reportes de ventas | Reportes por período exportables a Excel y PDF |

---

## Funcionalidades

### Autenticación y Seguridad
- Login y logout con ASP.NET Core Identity
- Roles diferenciados: Administrador y Cajero
- El administrador registra los usuarios cajeros
- Todas las rutas protegidas con `[Authorize]`
- Menú dinámico según el rol del usuario

### Gestión de Clientes
- Registro de clientes con datos completos
- Búsqueda por número de identificación
- Registro inline durante el proceso de venta
- Número de identificación único por cliente

### Gestión de Productos
- Creación y edición de productos
- Control de stock en tiempo real
- Código único por producto
- Indicador visual de estado del stock (disponible, bajo, agotado)

### Proceso de Venta
- Identificación obligatoria del cliente al iniciar la venta
- Fecha asignada automáticamente por el sistema (no editable)
- Múltiples productos por venta
- Validación de stock antes de agregar cada producto
- Resumen en tiempo real con subtotales y total general
- Transacción atómica con rollback en caso de error

### Reportes
- Reportes diarios, semanales, mensuales y anuales
- Exportación a Excel (.xlsx) con ClosedXML
- Exportación a PDF con iText7

---

## Tecnologías Utilizadas

| Tecnología | Versión | Uso |
|---|---|---|
| ASP.NET Core MVC | .NET 6 | Framework web principal |
| Entity Framework Core | 6.0.25 | ORM y acceso a datos |
| ASP.NET Core Identity | 6.0.25 | Autenticación y autorización |
| SQL Server | - | Base de datos relacional |
| ClosedXML | 0.102.1 | Generación de archivos Excel |
| iText7 | 7.2.5 | Generación de archivos PDF |
| Bootstrap | 5.3.2 | Framework CSS |
| Bootstrap Icons | 1.11.3 | Iconografía |
| JavaScript (Vanilla) | - | Lógica del cliente (AJAX, tabla dinámica) |

---

## Arquitectura y Patrones

El proyecto implementa una arquitectura en capas con los siguientes patrones de diseño:

```
???????????????????????????????????????????
?           CAPA DE PRESENTACIÓN          ?
?       Views (.cshtml) + ViewModels      ?
???????????????????????????????????????????
?           CAPA DE APLICACIÓN            ?
?     Controllers + ASP.NET Identity      ?
???????????????????????????????????????????
?           CAPA DE NEGOCIO               ?
?    ISaleService   ?   SaleService       ?
?    IReportService ?   ReportService     ?
???????????????????????????????????????????
?           CAPA DE DATOS                 ?
?   EF Core DbContext (Repository + UoW)  ?
?           SQL Server                    ?
???????????????????????????????????????????
```

### Patrones implementados
- **MVC** — Separación de responsabilidades entre modelo, vista y controlador
- **Service Layer** — Lógica de negocio aislada en servicios
- **Dependency Injection** — Servicios inyectados vía constructor
- **Repository Pattern** — Implícito mediante EF Core DbContext
- **Unit of Work** — Transacciones con BeginTransaction / Commit / Rollback
- **ViewModel Pattern** — DTOs entre controlador y vista
- **Strategy Pattern** — Selección de formato Excel o PDF en tiempo de ejecución

### Principios SOLID aplicados
- **S** — Single Responsibility: cada clase tiene una sola responsabilidad
- **O** — Open/Closed: servicios extensibles sin modificar código existente
- **L** — Liskov Substitution: controladores dependen de interfaces
- **I** — Interface Segregation: interfaces pequeñas y específicas
- **D** — Dependency Inversion: dependencia de abstracciones, no implementaciones

---

## Estructura del Proyecto

```
SuperMarketAntojitos/
?
??? Controllers/
?   ??? AccountController.cs
?   ??? AdminController.cs
?   ??? CustomersController.cs
?   ??? ProductsController.cs
?   ??? SalesController.cs
?   ??? ReportsController.cs
?
??? Data/
?   ??? ApplicationDbContext.cs
?   ??? DbSeeder.cs
?
??? Models/
?   ??? Entities/
?   ?   ??? ApplicationUser.cs
?   ?   ??? Customer.cs
?   ?   ??? Product.cs
?   ?   ??? Sale.cs
?   ?   ??? SaleDetail.cs
?   ??? ViewModels/
?       ??? LoginViewModel.cs
?       ??? RegisterViewModel.cs
?       ??? SaleViewModel.cs
?       ??? SaleDetailViewModel.cs
?       ??? ReportViewModel.cs
?
??? Services/
?   ??? Interfaces/
?   ?   ??? ISaleService.cs
?   ?   ??? IReportService.cs
?   ??? SaleService.cs
?   ??? ReportService.cs
?
??? Views/
?   ??? Account/
?   ?   ??? Login.cshtml
?   ?   ??? Register.cshtml
?   ?   ??? AccessDenied.cshtml
?   ??? Admin/
?   ?   ??? Index.cshtml
?   ??? Customers/
?   ?   ??? Index.cshtml
?   ?   ??? Create.cshtml
?   ?   ??? Edit.cshtml
?   ??? Products/
?   ?   ??? Index.cshtml
?   ?   ??? Create.cshtml
?   ?   ??? Edit.cshtml
?   ??? Sales/
?   ?   ??? Index.cshtml
?   ??? Reports/
?   ?   ??? Index.cshtml
?   ??? Shared/
?       ??? _Layout.cshtml
?       ??? _ValidationScriptsPartial.cshtml
?
??? wwwroot/
??? appsettings.json
??? Program.cs
??? README.md
```

---

## Modelo de Datos

![Modelo](imgages/ER-Diagram.png)

---

## Requisitos Previos

Antes de instalar el proyecto asegúrate de tener:

- [Visual Studio 2022](https://visualstudio.microsoft.com/) con el workload **ASP.NET and web development**
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) o SQL Server LocalDB (incluido con Visual Studio)
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (opcional, para explorar la base de datos)

---

## Instalación y Configuración

### 1. Clonar o descargar el proyecto

```bash
git clone https://github.com/Godie84/SuperMarketAntojitos.git
```

O descarga el ZIP y extráelo en tu carpeta de proyectos.

### 2. Abrir en Visual Studio

Abre el archivo `SuperMarketAntojitos.sln` con Visual Studio 2022.

### 3. Instalar los paquetes NuGet

Abre la **Package Manager Console** (Tools ? NuGet Package Manager ? Package Manager Console) y ejecuta:

```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.25
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 6.0.25
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 6.0.25
Install-Package ClosedXML -Version 0.102.1
Install-Package itext7 -Version 7.2.5
```

### 4. Configurar la cadena de conexión

Abre `appsettings.json` y ajusta la cadena de conexión según tu entorno:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Antojitos_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Si usas SQL Server local con usuario y contraseña:

```json
"DefaultConnection": "Server=localhost;Database=AntojitosDB;User Id=sa;Password=TuContraseña;TrustServerCertificate=True"
```

### 5. Crear la base de datos

En la **Package Manager Console** ejecuta:

```bash
Add-Migration InitialCreate
Update-Database
```

### 6. Ejecutar el proyecto

Presiona **F5** o el botón de Play. Al iniciar, el sistema crea automáticamente los roles y el usuario administrador.

---

## Credenciales por Defecto

```
Administrador
?????????????????????????????
Email:    admin@antojitos.com
Password: Febrero2026
```

---

## Roles del Sistema

| Rol | Descripción | Acceso |
|---|---|---|
| **Admin** | Gerente del supermercado | Productos, Clientes, Reportes, Registro de cajeros |
| **Cashier** | Personal de caja | Ventas, Consulta de clientes |

### Flujo de registro de usuarios

```
El administrador inicia sesión
        ?
Accede al panel de administración
        ?
Registra al cajero con nombre, correo y contraseña
        ?
El cajero recibe el rol Cashier automáticamente
        ?
El cajero puede iniciar sesión y registrar ventas
```

---

## Módulos del Sistema

### Proceso de Venta
```
1. Cajero inicia sesión
2. Ingresa número de identificación del cliente
   ??? Cliente existe    ? cargar datos y continuar
   ??? Cliente no existe ? registrar cliente y continuar
3. Sistema asigna fecha automáticamente (no editable)
4. Cajero selecciona producto y cantidad
   ??? Stock suficiente  ? agregar a la venta
   ??? Stock insuficiente ? mostrar error
5. Repetir paso 4 para cada producto
6. Cajero revisa resumen (productos, subtotales, total)
7. Cajero confirma la venta
   ??? Éxito ? descontar stock, guardar venta
   ??? Error ? rollback de toda la transacción
```

---

## Reportes

Los reportes incluyen la siguiente información:

| Campo | Descripción |
|---|---|
| ID de venta | Identificador único de la venta |
| Fecha | Fecha y hora de la venta |
| Cliente | Nombre completo del cliente |
| Identificación | Número de identificación del cliente |
| Producto | Nombre del producto vendido |
| Cantidad | Unidades vendidas |
| Valor unitario | Precio al momento de la venta |
| Subtotal | Cantidad × Valor unitario |
| Total venta | Total de la transacción completa |

### Períodos disponibles
- **Diario** — ventas del día seleccionado
- **Semanal** — ventas de la semana del día seleccionado
- **Mensual** — ventas del mes del día seleccionado
- **Anual** — ventas del año del día seleccionado

---