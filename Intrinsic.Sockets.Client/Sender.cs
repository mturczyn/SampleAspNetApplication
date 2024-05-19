using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Intrinsic.Sockets.Server.DTOs;
using Intrinsic.Utis;

namespace Intrinsic.Sockets.Client;

public static class Sender
{
    public static async Task SendToServerAsync(
        IPEndPoint ipEndPoint, 
        CancellationToken cancellationToken)
    {
        // Example data object to serialize and send
        var dataObject = new ServerRequestDto(
            "Chris Doe",
            30,
            "chrisdoe@example.com");

        // Serialize the object to a JSON string
        string jsonString = JsonSerializer.Serialize(dataObject);

        // Convert the JSON string to a byte array
        byte[] byteData = Encoding.UTF8.GetBytes(jsonString);

        // Create a TCP/IP socket
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Connect to the remote endpoint
        await clientSocket.ConnectAsync(ipEndPoint);

        Logger.Log("Sending data to server");

        // Send the serialized data to the server
        int bytesSent = await clientSocket.SendAsync(byteData, cancellationToken);

        Logger.Log("Sent data to server");
    }
}
