using System.Text.Json;

using MessagePack;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Messaging;
using RabbitMQ.Messaging.Models;

using NotificationService.Managers.Interfaces;

namespace NotificationService.Managers;

public class MessageReceiveManager(IRabbitMqConnection baseConnection, IMailManager mailManager)
    : IMessageReceiveManager
{
    private static int _messageCounter;

    public async Task ReceiveMessageAsync(string queue, TimeSpan messageDelay, CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(baseConnection.Channel);
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var messageNumber = Interlocked.Increment(ref _messageCounter);

            var body = ea.Body.ToArray();
            var obj = MessagePackSerializer.Deserialize<UserUpdateMessage>(body);
            var json = JsonSerializer.Serialize(obj, jsonOptions);
            try
            {
                Console.WriteLine($" [x] #{messageNumber} Received {json} [{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]");
                var response = await mailManager.SendEmailAsync(obj);
                if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
                    Console.WriteLine($" [x] Error send message: {response.ErrorMessage}");

                if (stoppingToken.IsCancellationRequested)
                    throw new OperationCanceledException("Processing cancelled.");

                await baseConnection.Channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [x] Error processing message: {ex.Message}");
                await baseConnection.Channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
            finally
            {
                if (messageDelay != default)
                    await Task.Delay(messageDelay, stoppingToken).WaitAsync(stoppingToken);
            }
        };

        await baseConnection.Channel.BasicConsumeAsync(
            queue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);
    }
}