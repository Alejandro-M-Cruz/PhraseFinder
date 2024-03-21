using PhraseFinderService;

// Instantiate the Service wrapper specifying the binding and optionally the Endpoint URL. The BasicHttpBinding could be used instead.
var client = new EchoServiceClient(EchoServiceClient.EndpointConfiguration.WSHttpBinding_IEchoService, "https://localhost:5001/EchoService/WSHttps");

var simpleResult = await client.EchoAsync("Hello");
Console.WriteLine(simpleResult);

var msg = new EchoMessage() { Text = "Hello2" };
var msgResult = await client.ComplexEchoAsync(msg);
Console.WriteLine(msgResult);
