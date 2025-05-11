using System.Net;
using System.Net.Sockets;
using System.Text;

const string serverIp = "127.0.0.1";
const int port = 8080;


using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    socket.Bind(endpoint);
    socket.Listen();

    Console.WriteLine($"Server started at {serverIp}:{port}");

    await RunProcessingAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

async Task RunProcessingAsync()
{
    while (true)
    {
        Socket remoteSocket = await socket.AcceptAsync();
        Console.WriteLine("Connection opened...");

        byte[] buffer = new byte[1024];
        int byteCount = 0;
        string message = string.Empty;

        do
        {
            byteCount = await remoteSocket.ReceiveAsync(buffer);
            message += Encoding.UTF8.GetString(buffer, 0, byteCount);
        } while (remoteSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message}");

        await Task.Delay(2000);
        string response = "Hello from server! All OK!!!";
        await remoteSocket.SendAsync(Encoding.UTF8.GetBytes(response));

        remoteSocket.Shutdown(SocketShutdown.Both);
        remoteSocket.Close();

        Console.WriteLine("Connection closed...");
    }
}






