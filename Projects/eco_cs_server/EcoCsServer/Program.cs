using System.Threading.Tasks;
using System.Collections.Generic;
using EcoCsServer.Services;
using EcoCsServer;

class Program
{
    static async Task Main(string[] args)
    {
        // Queue<int> queue = new Queue<int>();
        Queue<MapData> queue = new Queue<MapData>();

        Task serverTask = StartServer(queue:queue);
        Task<string> taskA = Func(name:"A", delay:2100, nLoop:600);
        Task<string> taskB = Func(name:"B", delay:3100, nLoop:400);
        Task<string> taskC = Func(name:"C", delay:6100, nLoop:200);

        for(int i=0; i<10000; i++)
        {
            Console.WriteLine("count: " + queue.Count);
            if(queue.Count > 0)
            {
                MapData data = queue.Dequeue();
                Console.WriteLine("== AgentLocs: " + data.AgentLocs);
                Console.WriteLine("== AgentLocs[0].Row: " + data.AgentLocs[0].Row);
            }
            await Task.Delay(200);
        }

        string a = await taskA;
        string b = await taskB;
        string c = await taskC;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
    }

    // private static async Task StartServer(Queue<int> queue)
    private static async Task StartServer(Queue<MapData> queue)
    {
        // builder
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddGrpc();
        builder.Services.AddSingleton(queue);

        // app
        var app = builder.Build();
        app.MapGrpcService<DataReceiver>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        // run
        Console.WriteLine("Start Server");
        await app.RunAsync();
        Console.WriteLine("Quit Server");
    }

    private static async Task<string> Func(string name, int delay, int nLoop)
    {
        for(int i=0; i<nLoop; i++)
        {
            await Task.Delay(delay);
            Console.WriteLine("Func{0}: {1:f1}s", name, (i+1)*delay/1000f);
        }

        return name;
    }

    // private static async Task<string> Func2(string name, int delay, int nLoop, List<int> arr, string arrName)
    // {
    //     for(int i=0; i<nLoop; i++)
    //     {
    //         await Task.Delay(delay);
    //         Console.WriteLine("Func{0}: {1:f1}s", name, (i+1)*delay/1000f);
    //         PrintList(arr, arrName);
    //     }

    //     return name;
    // }


    // private static void PrintList(List<int> nums, string name)
    // {
    //     Console.Write(name + ": ");
    //     foreach(var value in nums)
    //     {
    //         Console.Write(value + " ");
    //     }
    //     Console.WriteLine();
    // }
}
