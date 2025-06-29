using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MediatorCore.Extensions;

public interface IMediatorConfiguration
{
    IServiceCollection AddValidators(Assembly fromAssembly);
    IServiceCollection AddEventHandlers(Assembly fromAssembly);
}
