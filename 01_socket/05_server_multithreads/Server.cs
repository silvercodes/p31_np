using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _05_server_multithreads;

internal class Server: IAsyncDisposable
{
    private int backlog;
    public string Ip { get; }
    public int Port { get; }
    public IPEndPoint IPEndPoint { get; set; }
    public Socket ServerSocket { get; set; }

    public Server(string ip, int port, int backlog = 10)
    {
        Ip = ip;
        Port = port;
        this.backlog = backlog;

        IPEndPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);

        ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerSocket.Bind(IPEndPoint);
    }

    public async Task StartAsync()
    {
        try
        {
            ServerSocket.Listen(backlog);
            Console.WriteLine($"Server started at {Ip}:{Port}");

            await HandleConnectionsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private async Task HandleConnectionsAsync()
    {
        while(true)
        {
            Socket remoteSocket = await ServerSocket.AcceptAsync();

            if (remoteSocket.RemoteEndPoint is IPEndPoint remoteEP)
            {
                await Console.Out.WriteLineAsync($"Connection opened for remote --> {remoteEP.Address}:{remoteEP.Port}");

                _ = Task.Run(() => HandleRequest(remoteSocket));
            }

        }
    }

    private void HandleRequest(Socket remoteSocket)
    {
        byte[] buffer = new byte[1024];
        int byteCount = 0;
        string message = string.Empty;

        do
        {
            byteCount = remoteSocket.Receive(buffer);
            message += Encoding.UTF8.GetString(buffer, 0, byteCount);
        } while (remoteSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message}");

        Thread.Sleep(10000);
        string response = "Hello from server! All OK!!!";
        remoteSocket.Send(Encoding.UTF8.GetBytes(response));

        remoteSocket.Shutdown(SocketShutdown.Both);
        remoteSocket.Close();

        Console.WriteLine("Connection closed...");
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Run(() =>
        {
            if (ServerSocket is not null)
            {
                ServerSocket.Dispose();
            }
        });
    }
}
