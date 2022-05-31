from __future__ import print_function

import grpc
import data_sender_pb2
import data_sender_pb2_grpc


def run(n: int):
    server_ip = "localhost"
    server_port = 5284
    with grpc.insecure_channel("{}:{}".format(server_ip, server_port)) as channel:
        stub = data_sender_pb2_grpc.DataSenderStub(channel)
        response = stub.SendData(data_sender_pb2.Data(num=n))
        # response = stub.SendDataNoReturn(data_sender_pb2.Data(num=n))
    # print("ai client received: " + str(response.state))
    print("ai client received: " + str(response))


if __name__ == "__main__":
    n = int(input())
    run(n)
