from __future__ import print_function

import logging

import grpc
import protofile_pb2
import protofile_pb2_grpc


def run():
    server_ip = "localhost"
    server_port = 50051
    with grpc.insecure_channel("{}:{}".format(server_ip, server_port)) as channel:
        stub = protofile_pb2_grpc.ServiceNameStub(channel)
        response = stub.SayHello(protofile_pb2.MessageInput(message_in='you'))
    print("Greeter client received: {}".format(response.message_out))


if __name__ == '__main__':
    logging.basicConfig()
    run()
