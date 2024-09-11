using System.Net.Sockets;
using System.Net;
using System.Text;

namespace BroadcastService
{
    public class BroadcastListenerService : BackgroundService
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _endPoint;
        private readonly ILogger<BroadcastListenerService> _logger;

        public BroadcastListenerService(ILogger<BroadcastListenerService> logger)
        {
            _logger = logger;
            _udpClient = new UdpClient(8888);
            _endPoint = new IPEndPoint(IPAddress.Any, 8888);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _udpClient.ReceiveAsync(stoppingToken);
                var message = Encoding.UTF8.GetString(result.Buffer);

                if (message == "DISCOVER_BACKEND")
                {
                    var response = Encoding.UTF8.GetBytes(GetLocalIPAddress());
                    await _udpClient.SendAsync(response, response.Length, result.RemoteEndPoint);
                }
            }
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ipAddress?.ToString() ?? "Unknown";
        }
    }

}
