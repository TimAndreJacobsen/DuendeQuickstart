using IdentityModel.Client;

Console.WriteLine("starting . . .");

// discover endpoints from metadata
var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    Console.ReadLine();
    return;
}

Console.WriteLine("Successfully grabbed discoverydocument");


// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "apiName"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine("Failed to authenticate");
    return;
}

Console.WriteLine("Successfully grabbed accesstoken");
Console.WriteLine(tokenResponse.AccessToken);