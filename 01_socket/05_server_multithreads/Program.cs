
using _05_server_multithreads;

const string ip = "127.0.0.1";
int port = 8080;

Server server = new Server(ip, port);
await server.StartAsync();
