using System.Collections.Concurrent;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Intrinsic.Sockets.Server.DTOs;
using Intrinsic.Utis;

namespace Intrinsic.Sockets.Server;

public static class Listener
{
    public static string ACK_MESSAGE = "Succesfully received data ({0} bytes)";
    public const byte BUFFER_SIZE = 200;
    public const byte RECEIVE_TIMEOUT_MS = 1;

    private readonly static CancellationTokenSource _cancellationTokenSource = 
        new CancellationTokenSource();
    private readonly static List<Task> _tasks = new();
    private readonly static ConcurrentBag<object> _receivedData = new();

    public static async Task TryReceive(int port, CancellationToken cancellationToken)
    {
        try
        {
            await Receive(port, cancellationToken);
        }
        catch (SocketException se) when (se.SocketErrorCode == SocketError.TimedOut)
        {
            Logger.Log($"Receiving timed out! Error Message: {se.Message}");
        }
        catch (SocketException se)
        {
            Logger.Log($"Socket error has happened! {se.Message}");
            Logger.Log($"Error Code: {se.SocketErrorCode}");
        }
    }

    private static async Task Receive(int port, CancellationToken cancellationToken)
    {
        using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.ReceiveTimeout = RECEIVE_TIMEOUT_MS;

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

        while (true)
        {
            try 
            {
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token,
                    cancellationToken);
                _tasks.Add(
                    HandleIncomingConnectionAsync(
                        await socket.AcceptAsync(linkedCts.Token), 
                        cancellationToken));
            }
            catch (Exception tce)
            {
                if (tce is TaskCanceledException or OperationCanceledException)
                {
                    Logger.Log($"Cancelled task! {tce.Message}");
                    break;
                }
                throw;
            }
        }

        await Task.WhenAll(_tasks);

        Logger.Log("Received data objects:");
        foreach (var dataObject in _receivedData)
        {
            Logger.Log(JsonSerializer.Serialize(dataObject));
        }
    }

    public static void Interrupt()
    {
        _cancellationTokenSource.Cancel();
    }

    private static async Task HandleIncomingConnectionAsync(Socket handler, CancellationToken cancellationToken)
    {
        try
        {
            Logger.Log("Accepted the connection");

            var received = await ReceiveResponseFromClientAsync(
                handler,
                cancellationToken);

            if (!received.Any())
            {
                return;
            }

            Logger.Log($"Received data, bytes received: {received.Length}");

            var dataObject = JsonSerializer.Deserialize<ServerRequestDto>(
                Encoding.UTF8.GetString(received));

            await handler.SendAsync(
                Encoding.UTF8.GetBytes(
                    string.Format(ACK_MESSAGE, received.Length)));

            if (dataObject is not null)
            {
                _receivedData.Add(dataObject);
            }
        }
        finally
        {
            handler.Dispose();
        }
    }

    private static async Task<byte[]> ReceiveResponseFromClientAsync(
        Socket handler, 
        CancellationToken cancellationToken)
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
