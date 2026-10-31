
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file and restore dependencies
COPY ["ClothingStoreAPI.csproj", "./"]
RUN dotnet restore "ClothingStoreAPI.csproj"

# Copy the remaining source code
COPY . .

# Publish the application
RUN dotnet publish "ClothingStoreAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ClothingStoreAPI.dll"]
