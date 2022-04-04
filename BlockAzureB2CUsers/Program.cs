using Azure.Identity;
using MSGraphServiceClient = Microsoft.Graph.GraphServiceClient;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MSGraphServiceClient>(implementationFactory =>
{
    var authProvider = new ClientSecretCredential(builder.Configuration["AzureAdB2C:TenantId"],
        builder.Configuration["AzureAdB2C:ClientId"],
        builder.Configuration["AzureAdB2C:Secret"]);
    return new MSGraphServiceClient(authProvider);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/BlockUser/{userObjectId}", async (string userObjectId, MSGraphServiceClient graphServiceClient) =>
{
    var selectedUser = await graphServiceClient.Users[userObjectId].Request().GetAsync();
    if (selectedUser == null)
    {
        return Results.NotFound();
    }
    selectedUser.AccountEnabled = false;
    await graphServiceClient.Users[userObjectId].Request().UpdateAsync(selectedUser);
    return Results.Ok();
})
.WithName("BlockAzureB2CUser");

app.Run();
