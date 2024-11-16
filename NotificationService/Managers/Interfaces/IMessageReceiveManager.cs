namespace NotificationService.Managers.Interfaces;

public interface IMessageReceiveManager
{
    Task ReceiveMessageAsync(string queueName, TimeSpan messageDelay, CancellationToken stoppingToken);
}