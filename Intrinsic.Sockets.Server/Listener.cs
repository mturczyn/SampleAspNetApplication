using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Intrinsic.Sockets.Server.DTOs;
using Intrinsic.Utis;

namespace Intrinsic.Sockets.Server;

public static class Listener
{
    public static async Task Receive(int port, CancellationToken cancellationToken)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var bufferSize = 8192;
        socket.SetSocketOption(
            SocketOptionLevel.Socket, 
            SocketOptionName.SendBuffer,
            bufferSize);

        socket.SetSocketOption(
            SocketOptionLevel.Socket, 
            SocketOptionName.ReceiveBuffer,
            bufferSize);

        LingerOption lingerOption = new LingerOption(true, 10);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption);

        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

        socket.Bind(localEndPoint);

        byte[] buffer = new byte[1024];

        socket.Listen();

        Logger.Log("Starting to accept the connections");

        var handler = await socket.AcceptAsync();

        Logger.Log("Accepted the connection");

        var received = await handler.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);

        Logger.Log("Received data");

        var dataObject = JsonSerializer.Deserialize<ServerRequestDto>(
            Encoding.UTF8.GetString(buffer[0..received]));
    }
}
