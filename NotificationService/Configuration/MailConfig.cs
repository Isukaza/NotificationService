using System.ComponentModel.DataAnnotations;
using Amazon;

namespace NotificationService.Configuration;

public static class MailConfig
{
    private static class Keys
    {
        private const string GroupName = "Mail";
        public const string MailKey = GroupName + ":Mail";
        public const string RegionKey = GroupName + ":Region";
        public const string AwsAccessKeyIdKey = GroupName + ":AwsAccessKeyId";
        public const string AwsSecretAccessKeyKey = GroupName + ":AwsSecretAccessKey";
    }

    public static class Values
    {
        public static readonly RegionEndpoint RegionEndpoint;
        public static readonly string AwsAccessKeyId;
        public static readonly string AwsSecretAccessKey;

        static Values()
        {
            var configuration = ConfigBase.GetConfiguration();
            RegionEndpoint = RegionEndpoint.GetBySystemName(configuration[Keys.RegionKey]);
            AwsAccessKeyId = configuration[Keys.AwsAccessKeyIdKey];
            AwsSecretAccessKey = configuration[Keys.AwsSecretAccessKeyKey];
        }
    }
}