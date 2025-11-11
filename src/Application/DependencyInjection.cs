using System.Reflection;
using Application.Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void RegisterApplicationUseCases(this IServiceCollection services)
    {
        services.AddRegisterApplicationBehaviours(Assembly.GetExecutingAssembly());
    }

    private static void AddRegisterApplicationBehaviours(this IServiceCollection services,
        Assembly assembly, Action<MediatRServiceConfiguration>? mediator = null)
    {
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);

            configuration.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            mediator?.Invoke(configuration);
        });

        services.AddHttpContextAccessor();
    }
}
