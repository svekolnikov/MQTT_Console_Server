using System.Net;
using MQTT_Console_Server;

Console.WriteLine("Start...");

var ip = "192.168.0.5";
var port = "1883";

var server = new Server(ip, port);
await server.RunAsync();
var serverClient = new ServerClient(ip, port);
await serverClient.CreateAsync();

Console.ReadLine();
