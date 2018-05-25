using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using YammerAuthMVCApp;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class YammerAuthExtensions
    {
         public static AuthenticationBuilder AddYammer(this AuthenticationBuilder builder)
            => builder.AddYammer(YammerDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddYammer(this AuthenticationBuilder builder, Action<YammerOptions> configureOptions)
            => builder.AddYammer(YammerDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddYammer(this AuthenticationBuilder builder, string authenticationScheme, Action<YammerOptions> configureOptions)
            => builder.AddYammer(authenticationScheme, YammerDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddYammer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<YammerOptions> configureOptions)
            => builder.AddOAuth<YammerOptions, YammerHandler>(authenticationScheme, displayName, configureOptions);
    }
}