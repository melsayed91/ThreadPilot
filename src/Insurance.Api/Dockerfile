FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/Insurance.Api/Insurance.Api.csproj src/Insurance.Api/
COPY src/Insurance.Application/Insurance.Application.csproj src/Insurance.Application/
COPY src/Insurance.Infrastructure/Insurance.Infrastructure.csproj src/Insurance.Infrastructure/
COPY src/Insurance.Domain/Insurance.Domain.csproj src/Insurance.Domain/
COPY src/Shared/Shared.csproj src/Shared/
COPY nuget.config ./

RUN dotnet restore src/Insurance.Api/Insurance.Api.csproj --nologo

COPY . .
RUN dotnet publish src/Insurance.Api/Insurance.Api.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Insurance.Api.dll"]
