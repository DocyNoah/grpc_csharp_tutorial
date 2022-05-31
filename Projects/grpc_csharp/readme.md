note: This readme is based on vs code.

## Install .Net 6.0 SDK 
- latest version
  - https://dotnet.microsoft.com/en-us/download
- version 6.0
  - https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- What is .NET?
  - https://docs.microsoft.com/ko-kr/dotnet/core/introduction

## Make your protocol buffers
```protobuf
syntax = "proto3";

option csharp_namespace = "{PROJECT_NAME}";

package {PACKAGE};

service {SERVICE} {
  rpc {FUNCTION} ({MESSAGE1}) returns ({MESSAGE2}) {}
}

message {MESSAGE1} {
  string {MESSAGE_IN} = 1;
}

message {MESSAGE2} {
  string {MESSAGE_OUT} = 1;
}
```
This is used for protocol buffers on the server or client.

## Make your server

### Step 1: Create a project
```commandline
dotnet new grpc -o {SERVER_PROJECT}
```

#### struct
- Protos/*.proto
- Services/{SERVICE_CLASS}.cs
- Program.cs

#### Program.cs
```csharp
using {SERVER_PROJECT}.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<{SERVICE_CLASS}>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
```

#### Services/{SERVICE}.cs
```csharp
using Grpc.Core;
using {SERVER_PROJECT};

namespace {SERVER_PROJECT}.Services;

public class {SERVICE_CLASS} : {SERVICE}.{SERVICE}Base
{
    private readonly ILogger<{SERVICE_CLASS}> _logger;
    public {SERVICE_CLASS}(ILogger<{SERVICE_CLASS}> logger)
    {
        _logger = logger;
    }

    public override Task<{MESSAGE2}> {FUNCTION}({MESSAGE1} request, ServerCallContext context)
    {
        return Task.FromResult(new {MESSAGE2}
        {
            {MESSAGE_OUT} = "Hello " + request.{MESSAGE_IN}
        });
    }
}

```

### Step 2: Make your server's protocol buffers
```protobuf
syntax = "proto3";

option csharp_namespace = "{SERVER_PROJECT}";

package {PACKAGE};

service {SERVICE} {
  rpc {FUNCTION} ({MESSAGE1}) returns ({MESSAGE2}) {}
}

message {MESSAGE1} {
  string {MESSAGE_IN} = 1;
}

message {MESSAGE2} {
  string {MESSAGE_OUT} = 1;
}
```

note: {MESSAGE_IN} and {MESSAGE_OUT} are snake_case in *.proto, but PascalCase in *.cs.


### Step 3: Compile your server's protocol buffers
```commandline
dotnet-grpc add-file {PROTO_FILE}
```
- remove
```commandline
dotnet-grpc remove {PROTO_FILE}
```

### Step 4: Configure your server's port
At the ```{SERVER_PROJECT}/Properties/launchSettings.json```
- ```"applicationUrl": "https://localhost:{SERVER_PORT}"```

note: It's important if it's http or https.


## Make your client

### Step 1: Create a project
```commandline
dotnet new console -o {CLIENT_PROJECT}
```

#### Program.cs
```csharp
using System.Threading.Tasks;
using Grpc.Net.Client;
using {CLIENT_PROJECT};

using var channel = GrpcChannel.ForAddress("https://{SERVER_IP}:{SERVER_PORT}");
var client = new {SERVICE}.{SERVICE}Client(channel);
var reply = await client.{FUNCTION}Async(
                  new {MESSAGE1} { {MESSAGE_IN} = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.{MESSAGE_OUT});
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```

### Step 2: Install package
```commandline
dotnet add {CLIENT_PROJECT}.csproj package Grpc.Net.Client
```
```commandline
dotnet add {CLIENT_PROJECT}.csproj package Google.Protobuf
```
```commandline
dotnet add {CLIENT_PROJECT}.csproj package Grpc.Tools
```

### Step 3: Make your client's protocol buffers
The client's protobuffer is the same as the server's protobuffer except for the namespace.
- Copy server's protocol buffers
- Change a line ```option csharp_namespace = "{CLIENT_PROJECT}";```

note: {MESSAGE_IN} and {MESSAGE_OUT} are snake_case in *.proto, but PascalCase in *.cs.  
note: Unlike the server, the client does not need to compile protocol buffers.

### Step 4: Edit the project file
edit {CLIENT_PROJECT}.csproj
```js
<ItemGroup>
    <Protobuf Include="Protos\greet.proto" GrpcServices="Client" />
</ItemGroup>
```

### Step 5: Configure the server's port and ip
At the ```{CLIENT_PROJECT}/Program.cs```
- ```using var channel = GrpcChannel.ForAddress("https://{SERVER_IP}:{SERVER_PORT}");```


## Run Command
```commandline
dotnet run
```



## Reference
- https://docs.microsoft.com/ko-kr/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-6.0&tabs=visual-studio-code
- https://docs.microsoft.com/ko-kr/aspnet/core/grpc/?view=aspnetcore-6.0
- https://docs.microsoft.com/ko-kr/aspnet/core/grpc/basics?view=aspnetcore-6.0
- https://docs.microsoft.com/ko-kr/aspnet/core/grpc/client?view=aspnetcore-6.0
- https://docs.microsoft.com/ko-kr/aspnet/core/grpc/dotnet-grpc?view=aspnetcore-6.0
- https://docs.microsoft.com/ko-kr/aspnet/core/grpc/troubleshoot?view=aspnetcore-6.0#mismatch-between-client-and-service-ssltls-configuration
- https://stackoverflow.com/questions/63130334/grpc-service-instantiated-per-call
