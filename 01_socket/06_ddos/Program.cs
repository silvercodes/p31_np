using System.Net;
using System.Net.Sockets;
using System.Text;

const string serverIp = "127.0.0.1";
const int port = 8080;


void Start()
{
    using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

    try
    {
        string? input = "admin looser!";

        socket.Connect(endpoint);
        socket.Send(Encoding.UTF8.GetBytes(input));

        byte[] buffer = new byte[1024];
        int byteCount = 0;
        string response = string.Empty;

        do
        {
            byteCount = socket.Receive(buffer);
            response += Encoding.UTF8.GetString(buffer, 0, byteCount);
        } while (socket.Available > 0);

        Console.WriteLine($"Response: {response}");

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

        Console.WriteLine("Connection closed...");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}


Console.ReadLine();
for (int i = 0; i < 1000; ++i)
{
    _ = Task.Run(Start);
}
Console.ReadLine();






