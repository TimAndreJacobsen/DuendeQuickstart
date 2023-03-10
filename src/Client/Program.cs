
using IdentityModel.Client;
using System.Text.Json;

// discover endpoints from metadata
var client = new HttpClient();

var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

Console.WriteLine("Successfully grabbed discoverydocument");


// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "weatherApi"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine(tokenResponse.ErrorDescription);
    Console.WriteLine("\n Failed to authenticate");
    return;
}

Console.WriteLine("Successfully grabbed token");
Console.WriteLine(tokenResponse.Json);
Console.WriteLine("\n \n");

Console.WriteLine("Calling api with token");
// Call the api with the access token
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("https://localhost:6001/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// get data from weather api
var weatherResponse = await apiClient.GetAsync("https://localhost:6001/WeatherForecast");

if (!weatherResponse.IsSuccessStatusCode)
{
    Console.WriteLine(weatherResponse.StatusCode);
    Console.WriteLine("failed to get weather forecast");
}
else
{
    var doc = JsonDocument.Parse(await weatherResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}