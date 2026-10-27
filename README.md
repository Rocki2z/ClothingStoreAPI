# ClothingStoreAPI

## Requirements

- .NET 10 SDK
- PostgreSQL

## Configure the database

Set the connection string in .NET User Secrets so database credentials stay out
of source control:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=ClothingStoreDB;Username=postgres;Password=YOUR_PASSWORD"
```

Replace `YOUR_PASSWORD` with your local PostgreSQL password.

## Run

```powershell
dotnet run
```

In the Development environment, interactive API documentation is available at
`/scalar/v1` and the OpenAPI document at `/openapi/v1.json` on the app's local
HTTP address.
