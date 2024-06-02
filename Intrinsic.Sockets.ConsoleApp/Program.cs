// See https://aka.ms/new-console-template for more information
using Intrinsic.Sockets.Client;
using Intrinsic.Sockets.Server;
using System.Net;

var port = 8080;
var messagesToSend = 10;
var sendTasks = new Task[messagesToSend];

Console.WriteLine("Testing sockets. Important to note:");
Console.WriteLine($"Receive timeout is set to {Listener.RECEIVE_TIMEOUT_MS} ms");
Console.WriteLine($"Send timeout is set to {Sender.SEND_TIMEOUT_MS} ms");

var receiveTask = Listener.TryReceive(port, default);

for (int i = 0; i < sendTasks.Length; i++)
{
    sendTasks[i] = Sender.TrySendToServerAsync(
        new IPEndPoint(IPAddress.Loopback, port), default);
}

// Wait a little for messages to be sent and processed.
await Task.Delay(500);

// Interrupt listener, as it will block the program
// from terminating and getting final results.
Listener.Interrupt();

await Task.WhenAll(sendTasks.Append(receiveTask));

Console.WriteLine("Finished!");
