using Grpc.Core;
using EcoCsServer;
using System.Collections.Generic;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace EcoCsServer.Services;

public class DataReceiver : AIEconomist.AIEconomistBase
{
    private readonly ILogger<DataReceiver> _logger;
    // private Queue<int> _queue;
    private Queue<MapData> _queue;

    // public DataReceiver(ILogger<DataReceiver> logger, Queue<int> queue)
    public DataReceiver(ILogger<DataReceiver> logger, Queue<MapData> queue)
    {
        _logger = logger;
        _queue = queue;
    }

    public override Task<Empty> SendWorldMap(MapData request, ServerCallContext context)
    {
        Console.WriteLine("Received Data");
        // _queue.Enqueue(request.WorldSize.Row);  // the type of the queue must be <MapData>, but now <int>
        _queue.Enqueue(request);
        return Task.FromResult(new Empty());
    }
}
