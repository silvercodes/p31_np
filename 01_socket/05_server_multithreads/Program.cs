
using _05_server_multithreads;

const string ip = "127.0.0.1";
int port = 8080;

await using Server server = new Server(ip, port, 100);
await server.StartAsync();
