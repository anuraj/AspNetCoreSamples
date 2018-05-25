namespace YammerAuthMVCApp
{
    public class YammerDefaults
    {
        public const string AuthenticationScheme = "Yammer";
        public static readonly string DisplayName = "Yammer";
        public static readonly string AuthorizationEndpoint = "https://www.yammer.com/oauth2/authorize";
        public static readonly string TokenEndpoint = "https://www.yammer.com/oauth2/access_token.json";
        public static readonly string UserInformationEndpoint = "https://www.yammer.com/api/v1/users/current.json";
    }
}
