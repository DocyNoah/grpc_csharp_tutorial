using System.Threading.Tasks;
using System.Collections.Generic;
using UnityServer.Services;


class Program
{
    static async Task Main(string[] args)
    {
        List<int> buffer = new List<int>();

        Task serverTask = StartServer(buffer: buffer);
        Task<string> taskA = Func(name:"A", delay:2100, nLoop:600);
        Task<string> taskB = Func(name:"B", delay:3100, nLoop:400);
        Task<string> taskC = Func(name:"C", delay:6100, nLoop:200);
        Task<string> taskD = Func2(name:"D", delay:1100, nLoop:1200, arr:buffer, arrName:"[buffer]");
        
        string a = await taskA;
        string b = await taskB;
        string c = await taskC;
        string d = await taskD;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);
    }

    private static async Task StartServer(List<int> buffer)
    {
        // builder
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddGrpc();
        builder.Services.AddSingleton(buffer);  // == DataSenderService(buffer:buffer);

        // app
        var app = builder.Build();
        app.MapGrpcService<DataSenderService>();
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

    private static async Task<string> Func2(string name, int delay, int nLoop, List<int> arr, string arrName)
    {
        for(int i=0; i<nLoop; i++)
        {
            await Task.Delay(delay);
            Console.WriteLine("Func{0}: {1:f1}s", name, (i+1)*delay/1000f);
            PrintList(arr, arrName);
        }

        return name;
    }


    private static void PrintList(List<int> nums, string name)
    {
        Console.Write(name + ": ");
        foreach(var value in nums)
        {
            Console.Write(value + " ");
        }
        Console.WriteLine();
    }
}
