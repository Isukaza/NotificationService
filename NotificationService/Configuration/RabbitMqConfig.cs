using Helpers;

namespace NotificationService.Configuration;

public static class RabbitMqConfig
{
    private static class Keys
    {
        private const string GroupName = "RabbitMq";
        public const string HostKey = GroupName + ":Host";
        public const string QueueKey = GroupName + ":Queue";
        public const string PortKey = GroupName + ":Port";
        public const string UsernameKey = GroupName + ":Username";
        public const string PasswordKey = GroupName + ":Password";
        public const string ThreadsKey = GroupName + ":Threads";
        public const string PrefetchMessagesKey = GroupName + ":PrefetchMessages";
        public const string MessageDelayKey = GroupName + ":MessageDelay";
    }

    public static class Values
    {
        public static string Host { get; private set; }
        public static string Queue { get; private set; }
        public static int Port { get; private set; }
        public static string Username { get; private set; }
        public static string Password { get; private set; }
        public static ushort Threads { get; private set; }
        public static ushort PrefetchMessages { get; private set; }
        public static TimeSpan MessageDelay { get; private set; }

        public static void Initialize(IConfiguration configuration, bool isDevelopment)
        {
            Host = DataHelper.GetRequiredString(configuration[Keys.HostKey], Keys.HostKey);
            Queue = DataHelper.GetRequiredString(configuration[Keys.QueueKey], Keys.QueueKey);

            Port = DataHelper.GetRequiredInt(configuration[Keys.PortKey], Keys.PortKey, 1, 65535);

            var rawUsername = isDevelopment
                ? configuration[Keys.UsernameKey]
                : Environment.GetEnvironmentVariable("RABBITMQ_USER");
            Username = DataHelper.GetRequiredString(rawUsername, Keys.UsernameKey, 3);

            var rawPassword = isDevelopment
                ? configuration[Keys.PasswordKey]
                : Environment.GetEnvironmentVariable("RABBITMQ_PASS");
            Password = DataHelper.GetRequiredString(rawPassword, Keys.PasswordKey, 32);

            Threads = DataHelper.GetRequiredUShort(
                configuration[Keys.ThreadsKey],
                Keys.ThreadsKey,
                1,
                8);
            
            PrefetchMessages = DataHelper.GetRequiredUShort(
                configuration[Keys.PrefetchMessagesKey],
                Keys.PrefetchMessagesKey,
                1,
                100);

            var valueMessageDelay = DataHelper.GetRequiredInt(
                configuration[Keys.MessageDelayKey],
                Keys.MessageDelayKey,
                0,
                10000);
            MessageDelay = TimeSpan.FromMilliseconds(valueMessageDelay);
        }
    }
}