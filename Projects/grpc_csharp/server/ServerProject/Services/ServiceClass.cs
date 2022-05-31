using Grpc.Core;
using ServerProject;

namespace ServerProject.Services;

public class ServiceClass : ServiceName.ServiceNameBase
{
    private readonly ILogger<ServiceClass> _logger;
    public ServiceClass(ILogger<ServiceClass> logger)
    {
        Console.WriteLine("Server On");
        _logger = logger;
    }

    public override Task<MessageOutput> SayHello(MessageInput request, ServerCallContext context)
    {
        Console.WriteLine("Call SayHello()");
        return Task.FromResult(new MessageOutput
        {
            MessageOut = "Hello " + request.MessageIn
        });
    }
}
