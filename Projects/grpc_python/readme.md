## Install
```commandline
pip install grpcio
```
```commandline
pip install grpcio-tools
```

## Make your protocol buffers
form)
```protobuf
syntax = "proto3";

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

## Compile your protocol buffers
form)
```commandline
python -m grpc_tools.protoc --proto_path={PROTO_DIR} --python_out={{PROTO_FILE}_pb2.py DST_DIR} --grpc_python_out={{PROTO_FILE}_pb2_grpc.py DST_DIR} {PROTO_DIR}/{PROTO_FILE}.proto
```

ex)  
From the ```grpc_py_cs/grpc_python/protos``` directory, run:
```commandline
python -m grpc_tools.protoc --proto_path=. --python_out=. --grpc_python_out=. protofile.proto
```

## {PROTO_FILE}_pb2.py
This file has {MESSAGE1}, {MESSAGE2}

## {PROTO_FILE}_pb2_grpc.py
This file has {SERVICE}, {FUNCTION}, add_{SERVICE}Servicer_to_server
- {SERVICE}Stub
  - {FUNCTION}
- {SERVICE}Servicer
  - {FUNCTION}
- add_{SERVICE}Servicer_to_server

## Make your server.py or client.py
server form)
```python
from concurrent import futures

import grpc
import {PROTO_FILE}_pb2
import {PROTO_FILE}_pb2_grpc


class HelloMachine({PROTO_FILE}_pb2_grpc.{SERVICE}Servicer):

    def {FUNCTION}(self, request, context):
        return {PROTO_FILE}_pb2.{MESSAGE2}({MESSAGE_OUT}="Hello, {}!".format(request.{MESSAGE_IN}))


def serve():
    server_port = {SERVER_PORT}
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    {PROTO_FILE}_pb2_grpc.add_{SERVICE}Servicer_to_server(HelloMachine(), server)
    server.add_insecure_port('[::]:{}'.format(server_port))
    server.start()
    server.wait_for_termination()
```

client form)
```python
from __future__ import print_function

import grpc
import {PROTO_FILE}_pb2
import {PROTO_FILE}_pb2_grpc

def run():
    server_ip = {SERVER_IP}
    server_port = {SERVER_PORT}
    with grpc.insecure_channel("{}:{}".format(server_ip, server_port)) as channel:
        stub = {PROTO_FILE}_pb2_grpc.{SERVICE}Stub(channel)
        response = stub.{FUNCTION}({PROTO_FILE}_pb2.{MESSAGE1}({MESSAGE_IN}='you'))
    print("Greeter client received: " + response.{MESSAGE_OUT})
```

- Server and Client use the same proto, *_pb2, *_pb2_grpc


## Reference
- https://grpc.io/docs/languages/python/