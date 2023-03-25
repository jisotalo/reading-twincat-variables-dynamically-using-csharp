/*
  Minimal example how to read TwinCAT 3 variables dynamically from C# without providing data type definitions.
  
  https://github.com/jisotalo/reading-twincat-variables-dynamically-using-csharp
*/
using Newtonsoft.Json;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:9000");

var app = builder.Build();

//Creating new client instance
var client = new AdsClient();
client.Connect("localhost", 851);

//Creating a symbol loader
SymbolLoaderSettings settings = new SymbolLoaderSettings(SymbolsLoadMode.DynamicTree);
IAdsSymbolLoader dynLoader = (IAdsSymbolLoader)SymbolLoaderFactory.Create(client, settings);

//Loading symbols - note that this is quite expensive operation
var symbols = (await dynLoader.GetSymbolsAsync(CancellationToken.None)).Symbols;

//Helper method for reading data
async Task<dynamic> ReadValue(string plcAddress) 
{
  var symbol = await (symbols[plcAddress] as DynamicSymbol).ReadValueAsync(CancellationToken.None);
  return symbol.Value;
}

//Adding device notification to symbol
var symbol = (symbols["GVL_Test.StructValue"] as DynamicSymbol);

symbol.NotificationSettings = new NotificationSettings(AdsTransMode.OnChange, 1000, 0);
symbol.ValueChanged += new EventHandler<ValueChangedEventArgs>((sender, e) =>
{
  Console.WriteLine($"Symbol {e.Symbol.InstancePath} changed: {JsonConvert.SerializeObject(e.Value)}");
});

//Default - reading GVL_Test.StructValue data
app.MapGet("/", async () =>
{
  var data = await ReadValue("GVL_Test.StructValue");
  return Results.Text(JsonConvert.SerializeObject(data), "application/json");
});

//Reading by variable name (anything)
app.MapGet("/read-value-by-name", async (string variableName) =>
{
  var data = await ReadValue(variableName);
  return Results.Text(JsonConvert.SerializeObject(data), "application/json");
});

//Simple write example
app.MapPost("/write-string-value", async ([FromBody] string stringValue) =>
{
  var symbol = (symbols["GVL_Test.StructValue.TextValue"] as DynamicSymbol);
  await symbol.WriteValueAsync(stringValue, CancellationToken.None);

  return Results.Ok();
});

app.Run();