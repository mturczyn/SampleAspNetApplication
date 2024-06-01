using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Intrinsic.Sockets.Server.DTOs;
using Intrinsic.Utis;
using System.Threading;

namespace Intrinsic.Sockets.Client;

public static class Sender
{
    public const byte CHUNK_SIZE = 7;

    public static async Task SendToServerAsync(
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

        // Connect to the remote endpoint
        await clientSocket.ConnectAsync(ipEndPoint);

        var bytesSent = await SendDataToServerAsync(
            clientSocket, 
            byteData, 
            true,
            cancellationToken);

        Logger.Log("Sending data to server");

        Logger.Log($"Sent data to server. Bytes sent: {bytesSent}");
    }

    public static async Task<int> SendDataToServerAsync(
        Socket clientSocket,
        byte[] dataToSend,
        bool chunked,
        CancellationToken cancellationToken)
    {
        // Send the serialized data to the server
        return chunked 
            ? await clientSocket.SendAsync(
                dataToSend
                    .Chunk(CHUNK_SIZE)
                    .Select(x => new ArraySegment<byte>(x))
                    .ToList())
            : await clientSocket.SendAsync(dataToSend, cancellationToken);
    }
}
