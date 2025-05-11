using System.Net;
using System.Net.Sockets;

namespace _05_server_multithreads;

internal class Server
{
    public string Ip { get; }
    public int Port { get; }
    public IPEndPoint IPEndPoint { get; set; }
    public Socket ServerSocket { get; set; }

    public Server(string ip, int port)
    {
        Ip = ip;
        Port = port;

        IPEndPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);

        ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerSocket.Bind(IPEndPoint);
    }

    public async Task StartAsync()
    {
        try
        {
            ServerSocket.Listen();
            Console.WriteLine($"Server started at {Ip}:{Port}");

            await HandleAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private async Task HandleAsync()
    {
        // accept connection

        // handle in thread
    }





}
