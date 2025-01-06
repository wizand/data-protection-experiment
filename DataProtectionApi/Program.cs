using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

using Scalar.AspNetCore;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddLogging();
string? redisHost = builder.Configuration["Redis:Host"];
string? redisPort = builder.Configuration["Redis:Port"];
string? redisPassword = builder.Configuration["Redis:Password"];

if ( redisHost == null || redisPort == null || redisPassword == null)
{
    throw new Exception("Redis configuration is missing");
}

var connectionMultiplexer = ConnectionMultiplexer.Connect(
    new ConfigurationOptions
    {
        EndPoints = { { redisHost, int.Parse(redisPort) } },
        Password = redisPassword,
        AsyncTimeout = 10000,
        ConnectTimeout = 10000,
        ConnectRetry = 3,
        HeartbeatInterval = TimeSpan.FromSeconds(8),
        AbortOnConnectFail = true,
        
    } );

// Add the connection multiplexer to the service collection so it can be injected into other services
//builder.Services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

builder.Services.AddDataProtection(
    o => {
        o.ApplicationDiscriminator = "shared-keys";
        }).PersistKeysToStackExchangeRedis(connectionMultiplexer)
        .SetDefaultKeyLifetime(TimeSpan.FromDays(10));


builder.Services.AddSingleton<ICustomDataProtector, CustomDataProtector>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapPost("/requestToken", TokenRequest ([FromServices] ICustomDataProtector d,  [FromBody]string accessString) =>
{

    string decryptedAccessString = d.Decrypt(accessString);

    try
    {
        TokenRequest tokenRequest = System.Text.Json.JsonSerializer.Deserialize<TokenRequest>(decryptedAccessString);
        AccessValidator av = new AccessValidator(tokenRequest);
        if (av.IsValid())
        {
            return tokenRequest;
        }
        return null;
    }
    catch (Exception e)
    {

    }

    return new TokenRequest();

})
.WithName("requestToken");


app.MapPost("/createRequestTokenString", ([FromServices] ICustomDataProtector d, [FromBody] TokenRequest tokenRequest) =>
{

    string serializedToken = System.Text.Json.JsonSerializer.Serialize(tokenRequest);
    string enryptedAccessString = d.Encrypt(serializedToken);
    return enryptedAccessString;
})
.WithName("createRequestTokenString");

app.MapPost("/encryptString", ([FromServices] ICustomDataProtector d, [FromBody] string plaintextString) =>
{

    string encryptedString = d.Encrypt(plaintextString);


    return encryptedString;

})
.WithName("encryptString");


app.MapPost("/decryptString", ([FromServices] ICustomDataProtector d, [FromBody] string encryptedString) =>
{

    string plaintextString = d.Encrypt(encryptedString);
    return plaintextString;

})
.WithName("decryptString");

app.Run();

record TokenRequest()
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
}


