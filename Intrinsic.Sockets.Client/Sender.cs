using Intrinsic.Sockets.Server.DTOs;
using Intrinsic.Utis;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Intrinsic.Sockets.Client;

public static class Sender
{
    public const byte CHUNK_SIZE = 7;
    public const byte SEND_TIMEOUT_MS = 1;

    public static async Task TrySendToServerAsync(
        IPEndPoint ipEndPoint,
        CancellationToken cancellationToken)
    {
        try
        {
            await SendToServerAsync(ipEndPoint, cancellationToken);
        }
        catch (SocketException se) when (se.SocketErrorCode == SocketError.ConnectionRefused)
        {
            Logger.Log($"Connection refused! Error Message: {se.Message}");
        }
        catch (SocketException se) when (se.SocketErrorCode == SocketError.TimedOut)
        {
            Logger.Log($"Sending timed out! Error Message: {se.Message}");
        }
        catch (SocketException se)
        {
            Logger.Log($"Socket error has happened! {se.Message}");
            Logger.Log($"Error Code: {se.SocketErrorCode}");
        }
    }

    private static async Task SendToServerAsync(
    IPEndPoint ipEndPoint,
    CancellationToken cancellationToken)
    {
        // Example data object to serialize and send
        var dataObject = new ServerRequestDto(
            "Chris Doel",
            30,
            "chrisdoe@example.com");

        // Serialize the object to a JSON string
        string jsonString = JsonSerializer.Serialize(dataObject);

        // Convert the JSON string to a byte array
        byte[] byteData = Encoding.UTF8.GetBytes(jsonString);

        // Create a TCP/IP socket
        using Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        clientSocket.SendTimeout = SEND_TIMEOUT_MS;

        // Connect to the remote endpoint
        await clientSocket.ConnectAsync(ipEndPoint);

        var bytesSent = await SendDataToServerAsync(
            clientSocket,
            byteData,
            true,
            cancellationToken);

        var responseBuffer = new byte[1024];
        var responseBytesReceived = await clientSocket.ReceiveAsync(
            responseBuffer,
            cancellationToken);

        Logger.Log($"Server acknowledged receiving data with message:\n\n{Encoding.UTF8.GetString(responseBuffer[0..responseBytesReceived])}\n");
                
        Logger.Log("Sending data to server");

        Logger.Log($"Sent data to server. Bytes sent: {bytesSent}");
    }

    private static async Task<int> SendDataToServerAsync(
        Socket clientSocket,
        byte[] dataToSend,
        bool chunked,
        CancellationToken cancellationToken)
    {
        // Send the serialized data to the server
        var sentBytes = chunked
            ? await clientSocket.SendAsync(
                dataToSend
                    .Chunk(CHUNK_SIZE)
                    .Select(x => new ArraySegment<byte>(x))
                    .ToList())
            : await clientSocket.SendAsync(dataToSend, cancellationToken);

        // In order to terminate receiving process on the server side.
        clientSocket.Shutdown(SocketShutdown.Send);

        return sentBytes;
    }
}
