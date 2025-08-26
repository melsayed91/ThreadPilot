using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Domain;

namespace Vehicles.Api.Extensions;

internal static class ProblemDetailsExtensions
{
    public static IServiceCollection AddApiProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }

    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(a => a.Run(async ctx =>
        {
            var err = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
            var problem = err switch
            {
                ValidationException ve => new ProblemDetails
                {
                    Title = "Validation error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))
                },
                DomainException de => new ProblemDetails
                {
                    Title = "Domain error",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = de.Message
                },
                _ => new ProblemDetails
                {
                    Title = "Server error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = err?.Message
                }
            };

            ctx.Response.StatusCode = problem.Status ?? 500;
            await ctx.Response.WriteAsJsonAsync(problem);
        }));

        return app;
    }
}
