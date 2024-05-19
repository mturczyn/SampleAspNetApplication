// See https://aka.ms/new-console-template for more information
using Intrinsic.Sockets.Client;
using Intrinsic.Sockets.Server;
using System.Net;

var port = 8080;

var receiveTask = Listener.Receive(port, default);
var sendTask = Sender.SendToServerAsync(
    new IPEndPoint(IPAddress.Loopback, port), default);

await Task.WhenAll(receiveTask, sendTask);

Console.WriteLine("Finished!");
