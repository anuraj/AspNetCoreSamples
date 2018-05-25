using Microsoft.AspNetCore.Authentication.OAuth;

namespace YammerAuthMVCApp
{
    public class YammerOptions : OAuthOptions
    {
        public YammerOptions()
        {
            AuthorizationEndpoint = YammerDefaults.AuthorizationEndpoint;
            TokenEndpoint = YammerDefaults.TokenEndpoint;
            UserInformationEndpoint = YammerDefaults.UserInformationEndpoint;
        }
    }
}
