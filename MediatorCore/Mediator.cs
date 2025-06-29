using Microsoft.Extensions.DependencyInjection;
using MediatorCore.Interfaces;

namespace MediatorCore;

internal sealed class Mediator
    : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        Type requestType = request.GetType();
        Type responseType = typeof(TResponse);
        Type handerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        dynamic? handler = _serviceProvider.GetService(handerType);
        _ = handler ?? throw new InvalidOperationException($"{handerType}: Handler not found");

        Type loggingBehaviorInterface = typeof(IPipelineBehavior<,>)
            .MakeGenericType(requestType, responseType);
        var behaviors = (IEnumerable<dynamic>)_serviceProvider
            .GetServices(loggingBehaviorInterface) ?? new List<dynamic>();

        Func<Task<TResponse>> next = () => handler.Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var currentNext = next;
            next = () => ((dynamic)behavior).Handle((dynamic)request, currentNext);
        }

        return await next();
    }

    public async Task Publish(IEvent @event, CancellationToken cancellationToken = default)
    {
        Type eventType = @event.GetType();
        Type handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

        dynamic? handler = _serviceProvider.GetRequiredService(handlerType);
        _ = handler ?? throw new InvalidOperationException($"{handlerType}: Event Handler not found");

        await handler.Handle((dynamic)@event, cancellationToken);
    }
}
