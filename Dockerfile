# =============================================================================
# AREA52 DOCKERFILE - Railway/Docker Deployment
# =============================================================================
# Multi-stage build voor optimale image grootte
# Updated: Force rebuild
# =============================================================================

# Stage 1: Build (.NET 8.0 SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopieer ALLES eerst
COPY . .

# Toon dotnet versie voor debugging
RUN dotnet --version

# Restore NuGet packages
RUN dotnet restore "Area52.sln"

# Build en publish - specificeer exact welk project
WORKDIR "/src"
RUN dotnet publish "Area52.Web/Area52.Web.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime (kleinere image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Kopieer gepubliceerde bestanden
COPY --from=build /app/publish .

# Railway gebruikt PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose poort
EXPOSE 8080

# Start de applicatie
ENTRYPOINT ["dotnet", "Area52.Web.dll"]
