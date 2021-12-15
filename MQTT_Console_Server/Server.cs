using System.Diagnostics;
using System.Net;
using System.Text;
using MQTTnet;
using MQTTnet.Server;

namespace MQTT_Console_Server
{
    public class Server
    {
        private readonly MqttServerOptionsBuilder _options;
        private readonly IMqttServer _server;

        public Server(string ip, string port)
        {
            var isIpValid = IPAddress.TryParse(ip, out var ipChecked);
            if (!isIpValid) throw new ArgumentException(nameof(ip));

            Console.WriteLine($"Creating server on {ip}:{port}");

            _options = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointBoundIPAddress(ipChecked)
                .WithDefaultEndpointPort(1883);

            _server = new MqttFactory().CreateMqttServer();

            _server.UseClientConnectedHandler(args =>
            {
                Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] [Client connected: {args.ClientId}]");
            });

            _server.UseClientDisconnectedHandler(args =>
            {
                Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] [Client disconnected: {args.ClientId}]");
            });

            _server.UseApplicationMessageReceivedHandler(args =>
            {
                Console.WriteLine(
                    $"[{DateTime.Now:h:mm:ss tt}] " +
                    $"[Client: {args.ClientId}] " +
                    $"[Topic: {args.ApplicationMessage.Topic}] " +
                    $"[Message: {Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}]");

            });
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Run server...");
            await _server.StartAsync(_options.Build());
        }
    }
}
