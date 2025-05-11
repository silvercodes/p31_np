

using System.Net;
using System.Net.Sockets;
using System.Text;

const string serverIp = "127.0.0.1";
const int port = 8080;

//const string serverIp = "172.20.10.5";
//const int port = 8080;

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    Console.Write(">>> ");
    string? input = Console.ReadLine();

    if (input is null)
        throw new ArgumentException("Message is empty");

    socket.Connect(endpoint);               // BLOCKING
    socket.Send(Encoding.UTF8.GetBytes(input));     // BLOCKING

    byte[] buffer = new byte[1024];
    int byteCount = 0;
    string response = string.Empty;

    do
    {
        byteCount = socket.Receive(buffer);                         // BLOCKING
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






