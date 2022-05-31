from concurrent import futures
import logging

import grpc
import protofile_pb2
import protofile_pb2_grpc


class HelloMachine(protofile_pb2_grpc.ServiceNameServicer):

    def SayHello(self, request, context):
        return protofile_pb2.MessageOutput(message_out="Hello, {}!".format(request.message_in))


def serve():
    server_port = 50051
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    protofile_pb2_grpc.add_ServiceNameServicer_to_server(HelloMachine(), server)
    server.add_insecure_port('[::]:{}'.format(server_port))
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    logging.basicConfig()
    serve()
