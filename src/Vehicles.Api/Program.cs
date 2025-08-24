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
    .AddInfrastructure();

var app = builder.Build();

app.UseApiSwagger(app.Environment);
app.UseApiExceptionHandler();

app.MapGet("/", () => Results.Ok("Vehicles API running"));
app.MapGet("/health", () => Results.Ok("ok"));

app.MapControllers();

app.Run();
