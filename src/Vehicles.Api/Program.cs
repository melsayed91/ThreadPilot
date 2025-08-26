using Microsoft.FeatureManagement;
using Vehicles.Api.Extensions;
using Vehicles.Application.UseCases.GetVehicleByReg;
using Vehicles.Application.UseCases.GetVehiclesBatch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeatureManagement();

builder.Services.AddControllers();

builder.Services
    .AddApiSwagger()
    .AddApiProblemDetails()
    .AddMediator(
        typeof(GetVehicleByRegQuery).Assembly,
        typeof(GetVehiclesBatchQuery).Assembly)
    .AddInfrastructure()
    .AddPersistence(builder.Configuration);

var app = builder.Build();

app.UseApiSwagger(app.Environment);
app.UseApiExceptionHandler();

app.MapGet("/", () => Results.Ok("Vehicles API running"));
app.MapHealthChecks("/health");

app.MapControllers();

app.MigrateDatabaseOnStartup();
app.Run();

#pragma warning disable CA1515
public partial class Program;
#pragma warning restore CA1515
