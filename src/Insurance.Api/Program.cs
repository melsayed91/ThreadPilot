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
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseApiSwagger(app.Environment);
app.UseApiExceptionHandler();

app.MapGet("/", () => Results.Ok("Insurance API running"));
app.MapGet("/health", () => Results.Ok("ok"));

app.MapControllers();

app.Run();

public partial class Program;
