using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterApplicationUseCases();
        services.RegisterApplicationAuthentication(configuration);
        services.RegisterApplicationExternalDependencies(configuration);
        services.RegisterApplicationApiServices();
        services.AddHealthChecks();
        
        var allowedOrigins =
            configuration.GetRequiredSection("CorsSettings:AllowedOrigins").Get<string[]>();
        
        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder.WithOrigins(allowedOrigins!).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
        
        services.AddSignalR(options => options.EnableDetailedErrors = environment.IsDevelopment());
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        app.UseExceptionHandler("/api/error");

        if (!environment.IsDevelopment())
        {
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        var swaggerEnabled = configuration.GetValue("Swagger:Enabled", true);
        if (swaggerEnabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"LetsTripTogether API {description.GroupName.ToUpperInvariant()}");
                }

                options.RoutePrefix = "swagger";
                options.DocumentTitle = "LetsTripTogether Public API";
            });
        }
        
        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
            endpoints.MapHub<Hubs.NotificationHub>("/hubs/notifications");
        });
    }
}
