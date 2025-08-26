using FluentValidation;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Extensions;

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
            var ex = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
            var baseEx = ex?.GetBaseException();

            var (status, title, detail) = baseEx switch
            {
                ValidationException ve => (
                    StatusCodes.Status400BadRequest,
                    "Validation error",
                    string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))
                ),
                DomainException de => (
                    StatusCodes.Status422UnprocessableEntity,
                    "Domain error",
                    de.Message
                ),
                HttpRequestException hre => (
                    StatusCodes.Status502BadGateway,
                    "Upstream error",
                    hre.Message
                ),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Server error",
                    baseEx?.Message ?? "Unexpected error"
                )
            };

            ctx.Response.StatusCode = status;
            await ctx.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = title,
                Status = status,
                Detail = detail
            });
        }));

        return app;
    }
}
