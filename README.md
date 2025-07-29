
# MicroservicePermissions

Este proyecto es un microservicio desarrollado en ASP.NET Core que permite gestionar permisos laborales para empleados, integrando tecnologías modernas como Kafka, Elasticsearch y SQL Server.

## Tecnologías utilizadas

- ASP.NET Core 8
- Entity Framework Core
- Kafka
- Elasticsearch & Kibana
- SQL Server
- AutoMapper
- FluentValidation
- Docker & Docker Compose
- MediatR
- Serilog

## Estructura del proyecto

```
MicroservicePermissions/
├── Application/
├── Domain/
├── Infrastructure/
├── MicroservicePermissions/         # Proyecto principal de la API
├── MicroservicePermissions.Test/    # Proyecto de pruebas unitarias
├── Dockerfile
├── docker-compose.yml
├── README.md
```

## Configuración de entorno

Asegúrate de tener instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) habilitado
- Git

## Variables de entorno

Configuradas en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=sqlserver;Database=PermissionsDb;User Id=SA;Password=Asdf1234$;Encrypt=False;TrustServerCertificate=True;"
},
"Kafka": {
  "BootstrapServers": "kafka:9092",
  "Topic": "operations-log"
},
"Elasticsearch": {
  "Uri": "http://elasticsearch:9200"
}
```

## Ejecución con Docker

### 1. Construir imagen

Desde la raíz del proyecto (donde está el Dockerfile):

```bash
docker build -t microservicepermissions ./MicroservicePermissions
```

### 2. Ejecutar con Docker Compose

```bash
docker-compose up -d
```

Esto levanta:

- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- SQL Server: `localhost:1433`
- Kafka: `localhost:9092`
- Elasticsearch: `http://localhost:9200`
- Kibana: `http://localhost:5601`

## Swagger

Una vez en ejecución, accede a la documentación Swagger en:

[http://localhost:5000/swagger](http://localhost:5000/swagger)

## Pruebas

Las pruebas unitarias están en el proyecto `MicroservicePermissions.Test`.

Para ejecutarlas:

```bash
dotnet test
```

---

## Contacto

Si tienes alguna duda o pregunta sobre esta prueba técnica, por favor contacta a:

**Nombre:** Daniel Quintero  
**Email:** [dquintero.ing@gmail.com](mailto:dquintero.ing@gmail.com)  
**GitHub:** [https://github.com/dsquintero](https://github.com/dsquintero)

Estaré disponible para responder cualquier consulta o proporcionar aclaraciones adicionales.
