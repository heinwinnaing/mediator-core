using MediatorCore.Extensions;
using MediatorCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace MediatorCore;

public static class DependencyInjection
{
    public static IServiceCollection AddMediatorCore(this IServiceCollection services,
        Assembly assembly,
        Action<IMediatorConfiguration>? configuration = null)
    {
        if (configuration is not null)
        {
            var mtrCfg = new MediatorExtensions(services);
            configuration(mtrCfg);
        }

        return services
            .AddMediatorService(assembly);
    }

    private static IServiceCollection AddMediatorService(this IServiceCollection services, Assembly fromAssembly)
    {
        Type handlerInterface = typeof(IRequestHandler<,>);
        var handlerServiceDescriptors = fromAssembly
            .GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false }
            )
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface))
            .Select(handler => ServiceDescriptor.Scoped(
                service: handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface),
                implementationType: handler)
            );
        services
            .AddScoped<IMediator, Mediator>()
            .TryAddEnumerable(handlerServiceDescriptors);
        return services;
    }
}
