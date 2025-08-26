FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/Vehicles.Api/Vehicles.Api.csproj src/Vehicles.Api/
COPY src/Vehicles.Application/Vehicles.Application.csproj src/Vehicles.Application/
COPY src/Vehicles.Infrastructure/Vehicles.Infrastructure.csproj src/Vehicles.Infrastructure/
COPY src/Vehicles.Domain/Vehicles.Domain.csproj src/Vehicles.Domain/
COPY src/Shared/Shared.csproj src/Shared/
COPY nuget.config ./

RUN dotnet restore src/Vehicles.Api/Vehicles.Api.csproj --nologo

COPY . .
RUN dotnet publish src/Vehicles.Api/Vehicles.Api.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Vehicles.Api.dll"]
