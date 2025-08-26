namespace Insurance.Api.Extensions;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IApplicationBuilder UseApiSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment()) return app;
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
