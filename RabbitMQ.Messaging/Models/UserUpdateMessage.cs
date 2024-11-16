using MessagePack;
using NotificationService.DAL.Models;

namespace RabbitMQ.Messaging.Models;

[MessagePackObject]
public class UserUpdateMessage
{
    [Key(0)]
    public required string UserEmail { get; init; }
    [Key(1)]
    public string UserName { get; init; }
    [Key(2)]
    public string OldValue { get; init; }
    [Key(3)]
    public string NewValue { get; init; }
    [Key(4)]
    public required TokenType ChangeType { get; init; }
    [Key(5)]
    public required string ConfirmationLink { get; init; }
}