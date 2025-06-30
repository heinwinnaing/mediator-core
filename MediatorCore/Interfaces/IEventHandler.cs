namespace MediatorCore.Interfaces;

public interface IEventHandler<T>
    where T : IEvent
{
    Task Handle(T @event, CancellationToken cancellationToken = default);
}
