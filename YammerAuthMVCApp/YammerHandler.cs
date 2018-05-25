using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace YammerAuthMVCApp
{
    public class YammerHandler : OAuthHandler<YammerOptions>
    {
        public YammerHandler(IOptionsMonitor<YammerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret },
                { "code", code }
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            if (response.IsSuccessStatusCode)
            {
                var payloadObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                var payload = new JObject
                {
                    ["access_token"] = payloadObject.Property("access_token").Value["token"],
                    ["token_type"] = "code",
                    ["refresh_token"] = payloadObject.Property("access_token").Value["token"],
                    ["expires_in"] = payloadObject.Property("access_token").Value["expires_at"]
                };

                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                var error = new StringBuilder();
                error.Append("OAuth token endpoint failure: ");
                error.Append("Status: " + response.StatusCode + ";");
                error.Append("Headers: " + response.Headers.ToString() + ";");
                error.Append("Body: " + await response.Content.ReadAsStringAsync() + ";");
                return OAuthTokenResponse.Failed(new Exception(error.ToString()));
            }
        }
    }
}
