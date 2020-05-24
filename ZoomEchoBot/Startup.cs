using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ZoomEchoBot
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to Echo Bot.");
                });

                endpoints.MapGet("/authorize", async context =>
                {
                    var zoombotId = configuration["zoom_bot_jid"];
                    await Task.Run(() => context.Response.Redirect($"https://zoom.us/launch/chat?jid=robot_{zoombotId}"));
                });

                endpoints.MapGet("/support", async context =>
                {
                    await context.Response.WriteAsync("Write to support@echo.bot");
                });

                endpoints.MapGet("/privacy", async context =>
                {
                    await context.Response.WriteAsync("This URL will tell you about echo bot privacy");
                });

                endpoints.MapGet("/terms", async context =>
                {
                    await context.Response.WriteAsync("This URL will tell you about echo bot terms");
                });

                endpoints.MapGet("/documentation", async context =>
                {
                    await context.Response.WriteAsync("Type something I will echo it for you.");
                });

                endpoints.MapGet("/zoomverify/verifyzoom.html", async context =>
                {
                    var zoomverificationcode = configuration["zoom_verification_token"];
                    await context.Response.WriteAsync(zoomverificationcode);
                });
                endpoints.MapPost("/echo", async context =>
                {
                    var cmd = string.Empty;
                    var accountId = string.Empty;
                    var toId = string.Empty;
                    using (var streamReader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        var requestPayload = await streamReader.ReadToEndAsync();
                        Console.WriteLine(requestPayload);
                        var requestPayloadObject = JObject.Parse(requestPayload);
                        cmd = requestPayloadObject["payload"]["cmd"].Value<string>();
                        accountId = requestPayloadObject["payload"]["accountId"].Value<string>();
                        toId = requestPayloadObject["payload"]["toJid"].Value<string>();
                    }

                    var clientId = configuration["zoom_client_id"];
                    var clientSecret = configuration["zoom_client_secret"];
                    var url = "https://api.zoom.us/oauth/token?grant_type=client_credentials";
                    var accessToken = string.Empty;
                    using (var httpClient = httpClientFactory.CreateClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes($"{clientId}:{clientSecret}")));
                        var responseMessage = await httpClient.PostAsync(url, null);
                        responseMessage.EnsureSuccessStatusCode();
                        var tokenResponse = await responseMessage.Content.ReadAsStringAsync();
                        var tokenResponseObject = JObject.Parse(tokenResponse);
                        accessToken = tokenResponseObject["access_token"].Value<string>();
                    }

                    var messageUrl = "https://api.zoom.us/v2/im/chat/messages";
                    using (var httpClient = httpClientFactory.CreateClient())
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        var echoMessage = new JObject();
                        echoMessage["account_id"] = accountId;
                        echoMessage["robot_jid"] = configuration["zoom_bot_jid"];
                        echoMessage["to_jid"] = toId;

                        var content = new JObject()
                        {
                            ["head"] = new JObject()
                            {
                                ["text"] = "Echo"
                            },
                            ["body"] = new JArray()
                            {
                                new JObject()
                                {
                                    ["type"] = "message",
                                    ["text"] = $"You sent {cmd}"
                                }
                            }
                        };
                        echoMessage["content"] = content;
                        Console.WriteLine(echoMessage.ToString());
                        var messageContent = new StringContent(echoMessage.ToString(), Encoding.UTF8, "application/json");
                        await httpClient.PostAsync(messageUrl, messageContent);
                    }
                });
                endpoints.MapPost("/deauthorize", async context =>
                {
                    var requestPayload = string.Empty;
                    using (var streamReader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        requestPayload = await streamReader.ReadToEndAsync();
                        Console.WriteLine(requestPayload);
                        var requestBody = JsonSerializer.Deserialize<dynamic>(requestPayload);
                    }
                });
            });
        }
    }
}
