
using System.Net;
using System.Net.Sockets;
using System.Text;


//const string localhost = "127.0.0.1";
//const string remotehost = "127.0.0.1";

const string localhost = "172.20.10.5";
const string remotehost = "172.20.10.5";

Console.Write("Enter a local port: ");
int localPort = Int32.Parse(Console.ReadLine());
Console.Write("Enter a remote port: ");
int remotePort = Int32.Parse(Console.ReadLine());


using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

_ = Task.Run(() =>
{
	try
	{
		socket.Bind(new IPEndPoint(IPAddress.Parse(localhost), localPort));

		while(true)
		{
            byte[] buffer = new byte[65535];        // МАКСИМАЛЬНЫЙ БУФЕР МАКСИМАЛЬНЫЙ!!!
            int byteCount = 0;
            string message = string.Empty;

            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            do
            {
                byteCount = socket.ReceiveFrom(buffer, ref remoteEP);           // BLOCKING
                message += Encoding.UTF8.GetString(buffer, 0, byteCount);
            } while (socket.Available > 0);

            if (remoteEP is IPEndPoint remoteEPWithInfo)
            {
                Console.Write($"FROM: {remoteEPWithInfo.Address}:{remoteEPWithInfo.Port}");
            }

            Console.WriteLine($" > {DateTime.Now.ToShortTimeString()}: {message}");

        }
	}
	catch (Exception ex)
	{
        Console.WriteLine($"ERROR: {ex.Message}");
	}
    finally
    {
        socket.Close();
    }
});


try
{
    while(true)
    {
        Console.Write("\n<<< ");
        string? input = Console.ReadLine();

        if (input is not null)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);

            socket.SendTo(data, new IPEndPoint(IPAddress.Parse(remotehost), remotePort));
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    socket.Close();
}