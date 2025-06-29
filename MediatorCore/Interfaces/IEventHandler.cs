namespace MediatorCore.Interfaces;

public interface IEventHandler<T>
    where T : IEvent
{
    Task Handle(IEvent @event, CancellationToken cancellationToken = default);
}
