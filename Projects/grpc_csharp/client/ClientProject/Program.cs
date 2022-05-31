using System.Threading.Tasks;
using Grpc.Net.Client;
using ClientProject;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:50051");
var client = new ServiceName.ServiceNameClient(channel);
var reply = await client.SayHelloAsync(
                  new MessageInput { MessageIn = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.MessageOut);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();