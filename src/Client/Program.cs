
using IdentityModel.Client;


Console.WriteLine("starting . . .");

// discover endpoints from metadata
var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    Console.ReadLine();
    return;
}

Console.WriteLine("end");

// wait for user input to exit console
Console.ReadLine();
