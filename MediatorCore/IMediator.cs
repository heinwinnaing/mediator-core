using MediatorCore.Interfaces;

namespace MediatorCore;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}
