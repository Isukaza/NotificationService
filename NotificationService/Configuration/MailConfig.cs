using Amazon;
using Helpers;

namespace NotificationService.Configuration;

public static class MailConfig
{
    private static class Keys
    {
        private const string GroupName = "Mail";
        public const string MailKey = GroupName + ":Mail";
        public const string RegionKey = GroupName + ":Region";
        public const string AwsAccessKeyIdKey = GroupName + ":AwsAccessKeyId";
        public const string AwsSecretAccessKey = GroupName + ":AwsSecretAccessKey";
    }

    public static class Values
    {
        public static string Mail { get; private set; }
        public static RegionEndpoint RegionEndpoint { get; private set; }
        public static string AwsAccessKeyId { get; private set; }
        public static string AwsSecretAccessKey { get; private set; }

        public static void Initialize(IConfiguration configuration, bool isDevelopment)
        {
            Mail = DataHelper.GetRequiredEmail(configuration[Keys.MailKey], Keys.MailKey);

            var rawRegionEndpoint = DataHelper.GetRequiredString(configuration[Keys.RegionKey], Keys.RegionKey);
            RegionEndpoint = RegionEndpoint.GetBySystemName(rawRegionEndpoint);

            var rawAwsAccessKeyId = isDevelopment
                ? configuration[Keys.AwsAccessKeyIdKey]
                : Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            AwsAccessKeyId = DataHelper.GetRequiredString(rawAwsAccessKeyId, Keys.AwsAccessKeyIdKey);

            var rawAwsSecretAccessKey = isDevelopment
                ? configuration[Keys.AwsSecretAccessKey]
                : Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            AwsSecretAccessKey = DataHelper.GetRequiredString(rawAwsSecretAccessKey, Keys.AwsSecretAccessKey);
        }
    }
}