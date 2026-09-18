<div align="center">

# 🧾 InvoiceHub

**Sistema de Facturación Full-Stack con Clean Architecture**

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor WASM](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![EF Core 8](https://img.shields.io/badge/EF%20Core-8.0-512BD4?logo=nuget&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

*Aplicación web de facturación que permite la gestión de clientes, productos y la emisión de facturas con cálculo automático de impuestos. Desarrollada como prueba técnica para **The Factory HKA**.*

</div>

---

## 📋 Tabla de Contenidos

- [Descripción del Proyecto](#-descripción-del-proyecto)
- [Arquitectura](#-arquitectura)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Prerrequisitos](#-prerrequisitos)
- [Instalación y Ejecución](#-instalación-y-ejecución)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Endpoints de la API](#-endpoints-de-la-api)
- [Decisiones Técnicas](#-decisiones-técnicas)
- [Autor](#-autor)

---

## 🎯 Descripción del Proyecto

**InvoiceHub** es una aplicación web backend-frontend para la gestión y emisión de facturas. El sistema cubre el flujo completo de ventas:

- ✅ **Gestión de Clientes** — CRUD completo con validaciones, búsqueda y soft delete.
- ✅ **Gestión de Productos** — CRUD completo con control de stock, código único y soft delete.
- ✅ **Emisión de Facturas** — Selección de cliente, adición de productos con cantidades, cálculo automático de subtotal, impuestos (IVA configurable) y total general.
- ✅ **Validaciones de Negocio** — Control de stock al facturar, campos obligatorios, documentos únicos, mensajes de error descriptivos.

El proyecto fue desarrollado como respuesta a la evaluación técnica de **The Factory HKA**, demostrando competencia en desarrollo full-stack con el ecosistema .NET.

---

## 🏗️ Arquitectura

El proyecto implementa **Clean Architecture** organizada en **5 capas** con una clara separación de responsabilidades:

```
┌───────────────────────────────────────────────────────┐
│                   InvoiceHub.Client                   │  ← Blazor WebAssembly (UI)
│                   (Presentación)                      │
├───────────────────────────────────────────────────────┤
│                   InvoiceHub.API                      │  ← ASP.NET Core Web API
│                   (Punto de Entrada)                  │
├───────────────────────────────────────────────────────┤
│                InvoiceHub.Application                 │  ← Servicios, DTOs, Validaciones
│                (Lógica de Aplicación)                 │
├───────────────────────────────────────────────────────┤
│               InvoiceHub.Infrastructure               │  ← EF Core, Repositorios, UoW
│                (Acceso a Datos)                       │
├───────────────────────────────────────────────────────┤
│                  InvoiceHub.Domain                    │  ← Entidades, Interfaces
│                  (Núcleo del Negocio)                 │
└───────────────────────────────────────────────────────┘
```

| Capa | Proyecto | Responsabilidad |
|------|----------|-----------------|
| **Domain** | `InvoiceHub.Domain` | Entidades de negocio (`Client`, `Product`, `Invoice`, `InvoiceDetail`), interfaces de repositorios y contratos (`IRepository<T>`, `IUnitOfWork`). No depende de ninguna otra capa. |
| **Application** | `InvoiceHub.Application` | Servicios de aplicación, DTOs, mapeos manuales, validaciones con FluentValidation y orquestación de casos de uso. Depende únicamente de Domain. |
| **Infrastructure** | `InvoiceHub.Infrastructure` | Implementación de repositorios, `DbContext` con EF Core 8, configuraciones Fluent API, migraciones y Unit of Work. Depende de Domain. |
| **API** | `InvoiceHub.API` | Controladores REST, middleware de excepciones global, configuración de CORS, Swagger y pipeline de la aplicación. |
| **Client** | `InvoiceHub.Client` | Interfaz de usuario en Blazor WebAssembly que consume la API vía `HttpClient`. UI responsiva con notificaciones interactivas. |

**¿Por qué Clean Architecture?** Garantiza que el dominio sea independiente de frameworks y bases de datos, facilita pruebas unitarias, y permite reemplazar infraestructura (e.g., cambiar SQL Server por PostgreSQL) sin afectar la lógica de negocio.

---

## 🛠️ Tecnologías Utilizadas

### Backend
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **C#** | 12 | Lenguaje principal |
| **.NET** | 8.0 (LTS) | Framework base |
| **ASP.NET Core Web API** | 8.0 | API RESTful |
| **Entity Framework Core** | 8.0 | ORM / Acceso a datos |
| **SQL Server** | LocalDB | Motor de base de datos |
| **FluentValidation** | 12.1 | Validaciones de entrada |
| **Swashbuckle** | 6.6 | Documentación Swagger/OpenAPI |

### Frontend
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **Blazor WebAssembly** | .NET 8 | SPA interactiva en el navegador |
| **HTML5 / CSS3** | — | Maquetación y estilos |
| **SweetAlert2** | — | Notificaciones y confirmaciones |
| **Bootstrap / CSS Custom** | — | Estilos responsivos |

---

## 📦 Prerrequisitos

Asegúrate de tener instalado lo siguiente antes de ejecutar el proyecto:

| Herramienta | Versión Mínima | Enlace de Descarga |
|-------------|:--------------:|:------------------:|
| **Visual Studio 2022** | 17.8+ | [Descargar](https://visualstudio.microsoft.com/downloads/) |
| **.NET 8 SDK** | 8.0.x | [Descargar](https://dotnet.microsoft.com/download/dotnet/8.0) |
| **SQL Server** | LocalDB / Express | [Descargar](https://www.microsoft.com/sql-server/sql-server-downloads) |

> **Nota:** Visual Studio 2022 incluye SQL Server LocalDB con la carga de trabajo *"ASP.NET and web development"*. Si ya tienes VS instalado con esa carga de trabajo, no necesitas instalar SQL Server por separado.

### Cargas de trabajo requeridas en Visual Studio:
- ✅ ASP.NET and web development
- ✅ .NET desktop development *(opcional)*

---

## 🚀 Instalación y Ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/InvoiceHub.git
cd InvoiceHub
```

### 2. Abrir la solución

Abre el archivo `InvoiceHub.sln` con **Visual Studio 2022**.

### 3. Configurar la cadena de conexión

Abre el archivo `InvoiceHub.API/appsettings.json` y verifica/modifica la cadena de conexión según tu entorno:

```json
{
  "ConnectionStrings": {
    "DbConnection": "Server=(localdb)\\mssqllocaldb;Database=InvoiceHubDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

> **LocalDB** viene incluido con Visual Studio y no requiere configuración adicional.  
> Si usas **SQL Server Express**, cambia el `Server` a: `Server=.\\SQLEXPRESS;`

### 4. Restaurar paquetes NuGet

Visual Studio restaura los paquetes automáticamente al abrir la solución. Si no lo hace:

```
Menú → Build → Rebuild Solution
```

O desde la **Package Manager Console**:

```powershell
dotnet restore
```

### 5. Aplicar migraciones de base de datos

La aplicación ejecuta las migraciones automáticamente al iniciar (`Database.Migrate()` en `Program.cs`). Sin embargo, si deseas aplicarlas manualmente:

**Desde la Package Manager Console** (proyecto predeterminado: `InvoiceHub.Infrastructure`):

```powershell
Update-Database -Project InvoiceHub.Infrastructure -StartupProject InvoiceHub.API
```

**O desde la terminal:**

```bash
dotnet ef database update --project InvoiceHub.Infrastructure --startup-project InvoiceHub.API
```

### 6. Configurar el inicio múltiple (API + Client)

Para que ambos proyectos arranquen simultáneamente:

1. Clic derecho sobre la **Solución** → `Configure Startup Projects...`
2. Seleccionar **Multiple startup projects**
3. Establecer en **Start**:
   - `InvoiceHub.API`
   - `InvoiceHub.Client`
4. Hacer clic en **OK**

> El repositorio ya incluye esta configuración en el archivo `InvoiceHub.slnLaunch.user`.

### 7. Ejecutar

Presiona **`F5`** o haz clic en **▶ Start** en Visual Studio.

| Servicio | URL | Perfil |
|----------|-----|--------|
| **API (Swagger)** | `https://localhost:7079/swagger` | HTTPS |
| **Client (Blazor)** | `https://localhost:7227` | HTTPS |

---

## 📂 Estructura del Proyecto

```
InvoiceHub/
├── InvoiceHub.sln
│
├── InvoiceHub.Domain/                      # 🔵 Capa de Dominio
│   ├── Common/
│   │   └── BaseEntity.cs                   #    Entidad base (Id, CreatedAt, UpdatedAt)
│   ├── Entities/
│   │   ├── Client.cs                       #    Entidad Cliente
│   │   ├── Product.cs                      #    Entidad Producto
│   │   ├── Invoice.cs                      #    Entidad Factura
│   │   └── InvoiceDetail.cs                #    Detalle de Factura
│   └── Interfaces/
│       ├── IRepository.cs                  #    Repositorio Genérico
│       ├── IClientRepository.cs            #    Contrato de Cliente
│       ├── IProductRepository.cs           #    Contrato de Producto
│       ├── IInvoiceRepository.cs           #    Contrato de Factura
│       └── IUnitOfWork.cs                  #    Contrato Unit of Work
│
├── InvoiceHub.Application/                 # 🟢 Capa de Aplicación
│   ├── Dtos/                               #    Data Transfer Objects
│   │   ├── Client/
│   │   ├── Product/
│   │   ├── Invoice/
│   │   └── Common/
│   ├── Interfaces/                         #    Contratos de Servicios
│   ├── Mappings/                           #    Mapeos manuales Entity ↔ DTO
│   ├── Services/                           #    Implementación de Servicios
│   ├── Validators/                         #    FluentValidation Validators
│   └── DependencyInjection.cs              #    Registro IoC de la capa
│
├── InvoiceHub.Infrastructure/              # 🟠 Capa de Infraestructura
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs         #    DbContext + Auditoría automática
│   │   └── Configurations/                #    Fluent API Configurations
│   ├── Repositories/                       #    Implementaciones de Repositorios
│   ├── UnitOfWork/                         #    Implementación de UoW
│   ├── Migrations/                         #    Migraciones de EF Core
│   └── DependencyInjection.cs              #    Registro IoC de la capa
│
├── InvoiceHub.API/                         # 🔴 Capa API
│   ├── Controllers/                        #    Controladores REST
│   │   ├── ClientController.cs
│   │   ├── ProductController.cs
│   │   └── InvoiceController.cs
│   ├── Middlewares/
│   │   └── ExceptionMiddleware.cs          #    Manejo global de excepciones
│   ├── Program.cs                          #    Punto de entrada + pipeline
│   └── appsettings.json                    #    Configuración (ConnectionString, Tax)
│
└── InvoiceHub.Client/                      # 🟣 Capa Cliente (Blazor WASM)
    ├── Pages/
    │   ├── Home.razor                      #    Dashboard principal
    │   ├── Clientes.razor                  #    Gestión de Clientes
    │   ├── Productos.razor                 #    Gestión de Productos
    │   └── Facturacion.razor               #    Emisión de Facturas
    ├── Services/                           #    Servicios HTTP (API consumers)
    ├── Models/                             #    DTOs del lado cliente
    ├── Layout/                             #    Componentes de layout
    ├── Shared/                             #    Componentes compartidos
    └── wwwroot/                            #    Archivos estáticos (CSS, JS)
```

---

## 🌐 Endpoints de la API

### Clientes — `/api/client`

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/client` | Obtener todos los clientes activos |
| `GET` | `/api/client/{id}` | Obtener cliente por ID |
| `POST` | `/api/client` | Crear nuevo cliente |
| `PUT` | `/api/client/{id}` | Actualizar cliente existente |
| `DELETE` | `/api/client/{id}` | Desactivar cliente (Soft Delete) |

### Productos — `/api/product`

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/product` | Obtener todos los productos activos |
| `GET` | `/api/product/{id}` | Obtener producto por ID |
| `POST` | `/api/product` | Crear nuevo producto |
| `PUT` | `/api/product/{id}` | Actualizar producto existente |
| `DELETE` | `/api/product/{id}` | Desactivar producto (Soft Delete) |

### Facturas — `/api/invoice`

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/invoice` | Obtener todas las facturas con detalles |
| `POST` | `/api/invoice` | Crear nueva factura (descuenta stock) |

---

## 🧩 Decisiones Técnicas

### 🏛️ Patrones de Diseño

| Patrón | Implementación | Beneficio |
|--------|----------------|-----------|
| **Generic Repository** | `IRepository<T>` + repositorios concretos | Elimina código repetitivo de acceso a datos y centraliza las operaciones CRUD. |
| **Unit of Work** | `IUnitOfWork` → `SaveChangesAsync()` | Garantiza la atomicidad de las transacciones. Múltiples operaciones en repositorios se persisten en un único commit. |
| **Soft Delete** | `IsActive` + `HasQueryFilter()` | Los registros nunca se eliminan físicamente. Los Global Query Filters de EF Core excluyen automáticamente registros inactivos de todas las consultas. |
| **DTO Pattern** | DTOs separados para Create, Update y Response | Desacopla la capa de presentación del dominio, controla la información expuesta y evita el sobreposteo. |
| **Dependency Injection** | Métodos `AddInfrastructure()` y `AddApplicationServices()` | Cada capa registra sus propias dependencias de forma modular, facilitando la mantenibilidad y las pruebas. |

### 🔍 Validaciones

- **FluentValidation**: Validaciones de DTOs de entrada (campos requeridos, longitudes máximas, formatos).
- **Validaciones de Negocio**: Verificación de stock disponible al facturar, unicidad de documento del cliente, unicidad de código de producto.
- **Middleware de Excepciones**: Captura centralizada de errores que retorna respuestas JSON consistentes al cliente.

### 📊 Base de Datos

- **Auditoría Automática**: El `DbContext` sobrescribe `SaveChangesAsync` para mantener `CreatedAt` y `UpdatedAt` actualizados automáticamente.
- **Fluent API**: Todas las configuraciones de entidades (tipos, restricciones, índices, relaciones) se definen con `IEntityTypeConfiguration<T>`, manteniendo los modelos limpios.
- **Migraciones incluidas**: Las migraciones de EF Core están versionadas en el repositorio y se ejecutan automáticamente al iniciar la API.
- **Impuesto configurable**: El porcentaje de IVA se lee desde `appsettings.json` (`TaxSettings:DefaultPercentage`), permitiendo ajustarlo sin modificar código.

---

## 👤 Autor

Desarrollado por **Miguel** como prueba técnica para **The Factory HKA** — Septiembre 2026.

---

<div align="center">

*Desarrollado con ❤️ y .NET 8*

</div>
