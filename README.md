# Prueba de Backend y Bases de Datos
API REST desarrollada en .NET 9 para gestionar equipos, empleados y préstamos de equipos.
El sistema permite registrar y consultar equipos y empleados, así como administrar préstamos y devoluciones, controlando la disponibilidad de los equipos.

---

## Tecnologías utilizadas

- .NET 9
- C#
- ASP.NET Core Web API
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- MySQL
- Swagger / OpenAPI

---

## Funcionalidades implementadas

### Equipos

- Listar todos los equipos.
- Consultar un equipo por su ID.
- Registrar un equipo nuevo.
- Actualizar un equipo existente.
- Eliminar un equipo existente.
- Validar que el nombre y el número de serie sean obligatorios durante el registro o actualización de un equipo.
- Validar que el estado del equipo sea `Disponible`, `Prestado` o `En Mantenimiento` durante el registro o actualización de un equipo.
- Responder con `404 Not Found` cuando el equipo solicitado no existe en la base de datos.

### Empleados

- Listar todos los empleados.
- Registrar un empleado nuevo.
- Validar que los datos del empleado al momento de realizar un registro o actualización sean obligatorios (Nombre, Documento, Área y Correo Eléctronico).

### Préstamos

- Registrar un préstamo relacionando un equipo con un empleado.
- Registrar automáticamente la fecha del préstamo.
- Cambiar el estado del equipo a `Prestado`.
- Impedir préstamos de equipos que no estén disponibles o que no existan.
- Registrar la devolución de un préstamo.
- Registrar automáticamente la fecha de devolución.
- Cambiar el estado del préstamo a `Devuelto`.
- Cambiar el estado del equipo nuevamente a `Disponible`.
- Listar únicamente los préstamos activos, mostrando el equipo y el empleado relacionados.

---

## Requisitos previos

Para ejecutar debidamente el programa, se debe tener instalado lo siguiente:

- .NET 9 SDK
- MySQL Server
- DBeaver o cualquier cliente compatible con MySQL
- Git para clonar el repositorio

## Instalación y ejecución

1. Clonar el repositorio desde GitHub

```
    https://github.com/sixthdam/Prueba-backend-y-base-de-datos.git
```

Entrar a la carpeta del proyecto ejecutando el siguiente comando en la terminal:
```
    cd PrestamosAPI
```

2. Restaurar las dependencias

Ejecutar desde la terminal el siguiente comando:
```
    dotnet restore
``` 

3. Configurar la base de datos

Ejecturar el archivo `Data/Prestamos.sql` en MySQL para crear la base de datos con su respectiva información inicial.

4. Configurar la conexión

Crear o configurar el archivo `appsettings.json` con la cadena de conexión correspondiente a la instalación local de MySQL (ver `appsettingsexample.json` como ejemplo). Se deben reemplazar los valores de usuario y contraseña de acuerdo con la configuración local de MySQL.

5. Ejecutar

Ejecutar el programa mediante la terminal con el siguiente comando:

```
    dotnet run
```
Una vez iniciada, la API puede consultarse mediante Swagger desde la dirección indicada por la aplicación a través de la terminal (http://localhost:XXXX/swagger).

---

## Estructura del proyecto

```
    PrestamosAPI/
    ├── Controllers/
    │   ├── EquiposController.cs
    │   ├── EmpleadosController.cs
    │   └── PrestamosController.cs
    ├── Data/
    │   ├── PrestamosContext.cs
    │   └── Prestamos.sql
    ├── Models/
    │   ├── Equipo.cs
    │   ├── Empleado.cs
    │   ├── Prestamo.cs
    │   └── Request.cs
    ├── Properties
    │   └── launchSettings.json
    ├── Services/
    │   └── PrestamoService.cs
    ├── appsettings.json
    ├── appsettingsexample.json
    ├── PrestamosAPI.csproj
    ├── PrestamosAPI.http
    ├── Program.cs
    ├── .gitignore
    └── README.md
```

### Descripción de la estructura

- **Controllers:** reciben las solicitudes HTTP y devuelven las respuestas de la API.
- **Data:** contiene el contexto de Entity Framework Core y el script SQL para crear la base de datos.
- **Models:** contienen las clases que representan las entidades y datos utilizados por el sistema.
- **Properties:** contiene la configuración utilizada para ejecutar el proyecto, como los perfiles de `launchSettings.json`.
- **Services:** contiene la lógica de negocio relacionada con los préstamos.
- **appsettings.json:** contiene la configuración de la aplicación, incluyendo la cadena de conexión a la base de datos.
- **appsettingsexample.json:** contiene la base de ejemplo para la correcta configuración del archivo `appsettings.json`
- **PrestamosAPI.csproj:** contiene la configuración del proyecto y las dependencias utilizadas.
- **PrestamosAPI.http:** contiene solicitudes HTTP para realizar pruebas de los endpoints desde VS Code.
- **Program.cs:** configura los servicios, la conexión a la base de datos, los controladores y Swagger.
- **.gitignore:** especifica los archivos y carpetas que Git debe excluir del repositorio.
- **README.md:** contiene la documentación del proyecto, sus requisitos, configuración y endpoints.

---

## Endpoints y Swagger

La API incluye Swagger/OpenAPI para facilitar la documentación y prueba de los endpoints.

Después de ejecutar el proyecto, se puede acceder a la interfaz de Swagger desde:

```
http://localhost:XXXX/swagger
```

Se pueden consultar y probar los siguientes endpoints disponibles para equipos, empleados y préstamos:

### Equipos

| Método | Endpoint            | Descripción                    |
| ------ | ------------------- | ------------------------------ |
| GET    | `/api/equipos`      | Lista todos los equipos.       |
| GET    | `/api/equipos/{id}` | Consulta un equipo por su ID.  |
| POST   | `/api/equipos`      | Registra un nuevo equipo.      |
| PUT    | `/api/equipos/{id}` | Actualiza un equipo existente. |
| DELETE | `/api/equipos/{id}` | Elimina un equipo.             |

### Empleados

| Método | Endpoint         | Descripción                 |
| ------ | ---------------- | --------------------------- |
| GET    | `/api/empleados` | Lista todos los empleados.  |
| POST   | `/api/empleados` | Registra un nuevo empleado. |

### Préstamos

| Método | Endpoint                       | Descripción                                        |
| ------ | ------------------------------ | -------------------------------------------------- |
| POST   | `/api/prestamos`               | Registra un nuevo préstamo.                        |
| PUT    | `/api/prestamos/{id}/devolver` | Registra la devolución de un préstamo.             |
| GET    | `/api/prestamos/activos`       | Lista los préstamos que aún no han sido devueltos. |

---

## Códigos de respuesta HTTP

La API utiliza códigos de estado HTTP para indicar el resultado de cada operación:

| Código            | Significado                     | Uso en la API                                                  |
| ----------------- | ------------------------------- | -------------------------------------------------------------- |
| `200 OK`          | Operación exitosa               | Consultas, actualizaciones y operaciones exitosas.             |
| `201 Created`     | Recurso creado                  | Registro de nuevos equipos y empleados.                        |
| `204 No Content`  | Operación exitosa sin contenido | Eliminación de un equipo.                                      |
| `400 Bad Request` | Solicitud inválida              | Datos obligatorios faltantes o reglas de negocio no cumplidas. |
| `404 Not Found`   | Recurso no encontrado           | Cuando el equipo, empleado o préstamo solicitado no existe.    |

---

## Seguridad y buenas prácticas

- Las credenciales de acceso a MySQL no fueron publicadas en el repositorio.
- El archivo `appsettings.json` está incluido en `.gitignore` para evitar subir información sensible.
- Los archivos generados por .NET, como `bin/` y `obj/`, también se excluyen del repositorio.
- La API valida los datos recibidos antes de realizar operaciones sobre la base de datos.




