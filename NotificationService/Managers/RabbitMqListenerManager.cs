using NotificationService.Configuration;
using NotificationService.Managers.Interfaces;

namespace NotificationService.Managers;

public class RabbitMqListenerManager(IMessageReceiveManager messageReceiveManager, IHostApplicationLifetime lifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForApplicationInitialization(lifetime, stoppingToken))
            return;
        
        try
        {
            await messageReceiveManager.ReceiveMessageAsync(
                RabbitMqConfig.Values.Queue,
                RabbitMqConfig.Values.MessageDelay,
                stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            lifetime.StopApplication();
        }
    }

    private static async Task<bool> WaitForApplicationInitialization(IHostApplicationLifetime applicationLifetime,
        CancellationToken cancellationToken)
    {
        var applicationStartedTaskCompletionSource = new TaskCompletionSource();
        await using var appStartedRegistration = applicationLifetime.ApplicationStarted.Register(() => 
            applicationStartedTaskCompletionSource.SetResult());

        var cancellationTaskCompletionSource = new TaskCompletionSource();
        await using var cancellationRegistration = cancellationToken.Register(() => 
            cancellationTaskCompletionSource.SetResult());

        var completedTask = await Task
            .WhenAny(applicationStartedTaskCompletionSource.Task, cancellationTaskCompletionSource.Task)
            .ConfigureAwait(false);

        return completedTask == applicationStartedTaskCompletionSource.Task;
    }
}