using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceA
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        // unary
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        // server streaming
        public override async Task SayHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var greeting = $"Hello {request.Name}";

            foreach (var c in greeting)
            {
                if (context.CancellationToken.IsCancellationRequested)
                    return;

                await responseStream.WriteAsync(new HelloReply { Message = $"{c}" });
                await Task.Delay(500);
            }
        }

        // client streaming
        public override async Task<HelloReply> StreamingFromClient(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            var overallMessage = "Hello ";
            await foreach (var message in requestStream.ReadAllAsync())
            {
                // process individual message;
                overallMessage += $"{message.Name}";
                Console.Write(message.Name);
            }

            return new HelloReply { Message = overallMessage }; // reply only sent when client ends stream
        }

        // bi-directional streaming
        public override async Task StreamingBothWays(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await foreach (var message in requestStream.ReadAllAsync())
            {
                await responseStream.WriteAsync(new HelloReply { Message = $"Hello {message.Name}" }); // send response for each request
            }
        }
    }
}
