using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Intrinsic.Sockets.Server.DTOs;
using Intrinsic.Utis;

namespace Intrinsic.Sockets.Server;

public static class Listener
{
    public const byte BUFFER_SIZE = 200;

    public static async Task Receive(int port, CancellationToken cancellationToken)
    {
        using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

        socket.Listen();

        Logger.Log("Starting to accept the connections");

        using var handler = await socket.AcceptAsync();

        Logger.Log("Accepted the connection");

        var received = await ReceiveResponseFromClientAsync(
            handler, 
            cancellationToken);

        Logger.Log($"Received data, bytes received: {received}");

        var dataObject = JsonSerializer.Deserialize<ServerRequestDto>(
            Encoding.UTF8.GetString(received));
    }

    private static async Task<byte[]> ReceiveResponseFromClientAsync(Socket handler, CancellationToken cancellationToken)
    {
        var response = new List<byte>();

        byte[] buffer = new byte[BUFFER_SIZE];

        int received;
        do
        {
            received = await handler.ReceiveAsync(
                buffer,
                SocketFlags.None,
                cancellationToken);
            
            Logger.Log($"Receiving batched payload from {handler.RemoteEndPoint}. Received {received} bytes.");

            response.AddRange(buffer[0..received]);
        } while (received > 0);

        return response.ToArray();
    }
}
