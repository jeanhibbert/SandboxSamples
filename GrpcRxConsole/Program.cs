using Grpc.Core;
using Grpc.Net.Client;
//using GrpcRxService;

namespace GrpcRxConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var channel = GrpcChannel.ForAddress("http://localhost:5000");
        //var client = new Greeter.GreeterClient(channel);
        //var cts = new CancellationTokenSource();
        //try
        //{


        //    using (var call = client.BiDirectional())
        //    {
        //        var responseReaderTask = Task.Run(async () =>
        //        {
        //            while (await call.ResponseStream.MoveNext())
        //            {
        //                Response message = call.ResponseStream.Current;
        //                Console.WriteLine("Received " + message.Message);
        //            }
        //        });

        //        var request = new Request();
        //        for (int i = 0; i < 10; i++)
        //        {
        //            request.ContentValue = i.ToString();
        //            Console.WriteLine("Sending " + request.ContentValue);
        //            await call.RequestStream.WriteAsync(request);
        //        }
        //        await call.RequestStream.CompleteAsync();
        //        await responseReaderTask;
        //    }
        //}
        //catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        //{
        //    Console.WriteLine("Stream cancelled.");
        //}
    }
}
