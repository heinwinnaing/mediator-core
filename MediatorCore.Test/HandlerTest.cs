using MediatorCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MediatorCore.Test;
public class TestCommand : IRequest<bool>
{ }

public class HandlerTest
{
    [Fact]
    public async Task TestCase1()
    {
        //arrange
        var services = new ServiceCollection();
        var mockHanlder = new Mock<IRequestHandler<TestCommand, bool>>();
        var command = new TestCommand();
        mockHanlder.Setup(h => h.Handle(command, default))
            .ReturnsAsync(true);

        services.AddSingleton(mockHanlder.Object);
        services.AddSingleton<IMediator, Mediator>();

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        //act
        var result = await mediator.Send(command);

        //assert
        Assert.True(result);
        mockHanlder.Verify(h => h.Handle(command, default), Times.Once);
    }
}

