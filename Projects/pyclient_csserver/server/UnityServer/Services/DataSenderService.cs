using Grpc.Core;
using UnityServer;
using System.Collections.Generic;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace UnityServer.Services;

public class DataSenderService : DataSender.DataSenderBase
{
    private readonly ILogger<DataSenderService> _logger;
    private List<int> _buffer;
    private int call_count = 0;

    public DataSenderService(ILogger<DataSenderService> logger, List<int> buffer)
    {
        _buffer = buffer;
        Console.WriteLine("Server Activated");
        _logger = logger;
    }

    public override Task<Status> SendData(Data request, ServerCallContext context)
    {
        Console.WriteLine("Call SendData");
        call_count += 1;
        Console.WriteLine("call_count: " + call_count);
        Console.WriteLine("context: " + context);

        _buffer.Add(request.Num);
        PrintList(_buffer, "buffer");

        return Task.FromResult(new Status{State = 10});
    }

    public override Task<Empty> SendDataNoReturn(Data request, ServerCallContext context)
    {
        Console.WriteLine("Call SendDataNoReturn");
        _buffer.Add(request.Num);
        Task.Delay(3000).Wait();
        Console.WriteLine("Call SendDataNoReturn========================");
        return Task.FromResult(new Empty());
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
