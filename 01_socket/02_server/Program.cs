﻿


using System.Net;
using System.Net.Sockets;
using System.Text;

//const string serverIp = "127.0.0.1";
//const int port = 8080;

const string serverIp = "172.20.10.5";
const int port = 8080;


using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    socket.Bind(endpoint);
    socket.Listen();

    Console.WriteLine($"Server started at {serverIp}:{port}");

    while(true)
    {
        Socket remoteSocket = socket.Accept();                // BLOCKING

        byte[] buffer = new byte[1024];
        int byteCount = 0;
        string message = string.Empty;

        do
        {
            byteCount = remoteSocket.Receive(buffer);
            message += Encoding.UTF8.GetString(buffer, 0, byteCount);
        } while (remoteSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {message}");

        Thread.Sleep(2000);
        string response = "Hello from server! All OK!!!";
        remoteSocket.Send(Encoding.UTF8.GetBytes(response));

        remoteSocket.Shutdown(SocketShutdown.Both);
        remoteSocket.Close();

        Console.WriteLine("Connection closed...");
    }



}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}






