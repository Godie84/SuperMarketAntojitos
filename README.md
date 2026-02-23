## 1. Analisis del problema actual

* Registro de ventas manual
* Descuadres de dinero
* Pérdida de control del inventario
    - Stock actual
    - Ventas por dia

* No hay historial de clientes
* hábitos de compra

### Objetivos del sistema
* Registrar ventas diarias.
* Controlar inventario en tiempo real.
* Identificar clientes.
* Generar reportes.

## 2. Actores y casos de uso

Insertar imagen

## 3. Requerimientos funcionales

### Autenticación y usuarios
* El cajero debe registrarse (Por confidencialidad el administrador registra el usuario del cajero).
* Debe iniciar sesión.
* Solo usuarios autenticados pueden vender
* Roles (cajero, administrador).
* Seguridad básica (login, logout).

### Gestión de clientes

### Reglas de negocio: 

* todo cliente debe estar registrado.
* Regla de negocio: No se puede vender sin cliente.

### Datos obligatorios del cliente:

* Número de identificación (único).
* Nombre.
* Apellido.
* Dirección.
* Teléfono.
* Correo electrónico.

### Comportamiento con el cliente al iniciar una venta:

* Se solicita el número de identificación.
* Si no existe, se debe registrar.
* Si existe, usar el cliente.


### Gestión de productos

### Reglas de negocio:
* Al ejecutar una venta se debe actualizar el stock.
* No se puede vender más de lo que hay.
* El stock debe quedar siempre consistente.

### El sistema debe permitir:
* Crear productos con:
* Código del producto (único)
* Nombre
* Valor unitario
* Unidades existentes (stock)

### Proceso de venta.

### Al iniciar una venta se debe aplicar la regla de negocio que se especifica en cliente

* Fecha: Se asigna automáticamente, No es editable
* Cliente: Debe existir o crearse.
* Productos: Se pueden agregar varios productos, Cada uno con cantidad.

### Durante la venta, el cajero debe ver:

* Lista de productos agregados.
* Precio por producto
* Subtotales
* Total general

## 3.5 Reportes

### El sistema debe permitir generar reportes:

* Formatos:
    * Excel.
    * PDF.

* Tipos:

    * Diarios.
    * Semanales.
    * Mensuales.
    * Anuales.

### Posibles campos a mostrar:

* Fecha
* Total de ventas
* Detalle por producto o cliente

## 4. Requerimientos no funcionales

### Tecnológicos

* Backend: ASP.NET (preferible .NET 6)
* Base de datos: SQL Server
* Frontend: Bootstrap
* Reportes: ReportViewer, Rotativa

## 5. Relacion de reglas de negocio

* No se puede vender sin cliente.
* No se puede vender sin stock suficiente.
* La fecha de la venta:
    * Es automática
    * No editable
* El stock se actualiza en cada venta.
* Un producto no puede tener stock negativo.