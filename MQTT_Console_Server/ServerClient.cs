using System.Diagnostics;
using System.Net;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;

namespace MQTT_Console_Server
{
    public class ServerClient
    {
        private readonly IMqttClientOptions _options;
        private IMqttClient _client;
        public ServerClient(string ip, string port)
        {
            var isIpValid = IPAddress.TryParse(ip, out var ipChecked);
            if (!isIpValid) throw new ArgumentException(nameof(ip));

            var isPortValid = int.TryParse(port, out var portResult);

            Console.WriteLine($"Creating server-client on {ip}:{port}");

            _options = new MqttClientOptionsBuilder()
                .WithClientId("server-client")
                .WithTcpServer(ipChecked.ToString(), portResult)
                .Build();
        }

        public async Task CreateAsync()
        {
            Console.WriteLine("Creating client...");
            _client = new MqttFactory().CreateMqttClient();

            try
            {
                Console.WriteLine("Server client connecting to server...");

                var clientConnectResult = await _client.ConnectAsync(_options, CancellationToken.None);

                var clientSubscribeResult = await _client
                    .SubscribeAsync(new TopicFilterBuilder().WithTopic("STM32F1W5500").Build());

                Console.WriteLine($"[{DateTime.Now:h:mm:ss tt}] [Server-client subscribed to: ]");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
