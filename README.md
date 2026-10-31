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

For Docker Compose, copy `.env.example` to `.env` and set a unique database
password and a JWT signing key of at least 32 characters. `.env` is ignored by
Git and must not be committed.

```powershell
Copy-Item .env.example .env
```

Start the API and PostgreSQL containers:

```powershell
docker compose up --build
```

The API is available at `http://localhost:5102`. Apply the EF Core migrations
to the database before testing database-backed endpoints.

## Run

```powershell
dotnet run
```

In the Development environment, interactive API documentation is available at
`/scalar/v1` and the OpenAPI document at `/openapi/v1.json` on the app's local
HTTP address.
