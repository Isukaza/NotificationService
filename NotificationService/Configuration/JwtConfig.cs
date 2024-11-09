using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NotificationService.Configuration;

public static class JwtConfig
{
    private static class Keys
    {
        private const string GroupName = "JWT";
        public const string IssuerKey = GroupName + ":Issuer";
        public const string AudienceKey = GroupName + ":Audience";
        public const string KeyKey = GroupName + ":Key";
    }

    public static class Values
    {
        public static readonly string Issuer;
        public static readonly string Audience;
        public static readonly SymmetricSecurityKey Key;

        static Values()
        {
            var configuration = ConfigBase.GetConfiguration();

            Issuer = configuration[Keys.IssuerKey];
            Audience = configuration[Keys.AudienceKey];
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[Keys.KeyKey] ?? string.Empty));
        }
    }
}