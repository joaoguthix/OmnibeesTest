using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace MedSyncFunction.Application.Services
{
    public class QueueService // : IQueueService configuração para fila se necessário
    {
        private readonly ServiceBusClient _client;
        private readonly ILogger<QueueService> _logger;

        public QueueService(ServiceBusClient client, ILogger<QueueService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task SendMessageAsync<T>(T message, string queueName)
        {
            try
            {
                var sender = _client.CreateSender(queueName);
                var jsonMessage = JsonSerializer.Serialize(message);
                var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));

                await sender.SendMessageAsync(serviceBusMessage);
                _logger.LogInformation($"Mensagem enviada para a fila '{queueName}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar mensagem para a fila '{queueName}': {ex.Message}");
                throw;
            }
        }
    }
}
