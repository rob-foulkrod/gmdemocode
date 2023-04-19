using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;

Console.WriteLine("Starting MSAL Demo");

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var clientId = configuration["ClientId"];
var tenantId = configuration["TenantId"];

Console.WriteLine("ClientId: " + clientId);
Console.WriteLine("TenantId: " + tenantId);

// var app = PublicClientApplicationBuilder.Create(clientId)
//     .WithTenantId(tenantId)
//     .WithDefaultRedirectUri()  // <-- http://localhost
//     .Build();

//     var scopes = new [] {
//         "user.read"
//     };

// var response = await app.AcquireTokenInteractive(scopes).ExecuteAsync();   

// Console.WriteLine("Success!");

// var token = response.AccessToken.Substring(0, response.AccessToken.Length - 10);

// // Console.WriteLine("Token: " + token + "...");

// var account = response.Account;  // JWT decoded

// foreach(var claim in response.ClaimsPrincipal.Claims)
// {
//     Console.WriteLine(claim.Type + ": " + claim.Value);
// }

var authenticator = new InteractiveBrowserCredential(tenantId, clientId);

var graphClient = new GraphServiceClient(authenticator);

var stream = await graphClient.Me.Photo.Content.GetAsync();

var file = new FileStream("photo.jpg", FileMode.Create, FileAccess.Write);

await stream.CopyToAsync(file);

Console.WriteLine("Success!");


