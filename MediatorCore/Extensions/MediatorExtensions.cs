using MediatorCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace MediatorCore.Extensions;
internal sealed class MediatorExtensions
    : IMediatorConfiguration
{
    private readonly IServiceCollection _services;
    public MediatorExtensions(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection AddValidators(Assembly fromAssembly)
    {
        var validators = fromAssembly
            .GetTypes()
            .Where(type =>
                type is { IsClass: true, IsAbstract: false, IsInterface: false, IsGenericTypeDefinition: false }
            )
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            .Select(validator => ServiceDescriptor.Transient(
                service: validator.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)),
                implementationType: validator));

        _services
             .TryAddEnumerable(validators);
        return _services;
    }    
}
