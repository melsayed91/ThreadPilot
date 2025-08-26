using Insurance.Api.Extensions;
using Insurance.Application.UseCases.GetInsuranceSummary;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeatureManagement();

builder.Services.AddControllers();

builder.Services
    .AddApiSwagger()
    .AddApiProblemDetails()
    .AddMediator(typeof(GetInsuranceSummaryQuery).Assembly)
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

var app = builder.Build();

app.UseApiSwagger(app.Environment);
app.UseApiExceptionHandler();

app.MapGet("/", () => Results.Ok("Insurance API running"));
app.MapHealthChecks("/health");

app.MapControllers();

app.MigrateDatabaseOnStartup();
app.Run();

#pragma warning disable CA1515
public partial class Program;
#pragma warning restore CA1515
