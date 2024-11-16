namespace NotificationService.Configuration;

public static class RabbitMqConfig
{
    private static class Keys
    {
        private const string GroupName = "RabbitMq";
        public const string HostKey = GroupName + ":Host";
        public const string QueueKey = GroupName + ":Queue";
        public const string PortKey = GroupName + ":Port";
        public const string UserKey = GroupName + ":Username";
        public const string PasswordKey = GroupName + ":Password";
        public const string ThreadsKey = GroupName + ":Threads";
        public const string PrefetchMessagesKey = GroupName + ":PrefetchMessages";
        public const string MessageDelayKey = GroupName + ":MessageDelay";
    }

    public static class Values
    {
        public static readonly string Host;
        public static readonly string Queue;
        public static readonly int Port;
        public static readonly string Username;
        public static readonly string Password;
        public static readonly ushort Threads;
        public static readonly ushort PrefetchMessages;
        public static readonly TimeSpan MessageDelay;

        static Values()
        {
            var configuration = ConfigBase.GetConfiguration();
            Host = configuration[Keys.HostKey];
            Queue = configuration[Keys.QueueKey];

            Port = int.TryParse(configuration[Keys.PortKey], out var port)
                ? port
                : 5672;

            Username = configuration[Keys.UserKey];
            Password = configuration[Keys.PasswordKey];

            Threads = ushort.TryParse(configuration[Keys.ThreadsKey], out var threads)
                ? threads
                : (ushort)1;

            PrefetchMessages =
                ushort.TryParse(configuration[Keys.PrefetchMessagesKey], out var prefetchMessages)
                    ? prefetchMessages
                    : (ushort)0;

            MessageDelay = TimeSpan.FromMilliseconds(
                int.TryParse(configuration[Keys.MessageDelayKey], out var delay) && delay > 0
                    ? delay
                    : 0
            );
        }
    }
}