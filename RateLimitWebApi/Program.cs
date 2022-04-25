using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
// Custom Client Id configuration

// builder.Services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();

builder.Services.AddInMemoryRateLimiting();

//IP Rate limiting

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "10s",
                Limit = 2
            }
        };
});

// IP Rate Limit configuration from appsettings.json.

// builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

// Client Id rate limiting

// builder.Services.Configure<ClientRateLimitOptions>(options =>
// {
//     options.EnableEndpointRateLimiting = true;
//     options.StackBlockedRequests = false;
//     options.HttpStatusCode = 429;
//     options.ClientIdHeader = "Client-Id";
//     options.GeneralRules = new List<RateLimitRule>
//         {
//             new RateLimitRule
//             {
//                 Endpoint = "*",
//                 Period = "10s",
//                 Limit = 2
//             }
//         };
// });


var app = builder.Build();

//IP Rate limiting

app.UseIpRateLimiting();

// Client Id rate limiting

// app.UseClientRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast").ProducesProblem(429).ProducesProblem(200);

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
