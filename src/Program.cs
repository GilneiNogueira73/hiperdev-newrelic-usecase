using NewRelic.Api.Agent;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = logger;
Random r = new Random();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/greet", () => "hello there");

app.MapGet("/bug", () =>
{
    try
    {
        var z = 0;
        var error = 1 / z;
    }
    catch (Exception e)
    {
        logger.Error($"Erro ao executar a função: {e.Message}");
        return Results.BadRequest("Not OK");
    }
    return Results.Ok("OK");
});

//app.MapGet("/lazy", () =>
//{
//    var x = 0;
//    try
//    {
//        var z = 10;
//        var y = 20;
//        Thread.Sleep(10000);
//        x = z * y;
//    }
//    catch (Exception e)
//    {
//        logger.Error($"Erro ao executar a função: {e.Message}");
//        return Results.BadRequest("Not OK");
//    }
//    return Results.Ok($"O resultado da função é: {x}");
//});

//app.MapGet("/greetAnotherServer", async () => {
//    try
//    {
//        var client = new HttpClient();
//        var response = await client.GetAsync("http://hiperdev-api-rodrigo-service/hello");
//        var answer = response.Content.ReadAsStringAsync().Result;

//        logger.Information($"O servidor respondeu: {answer}");
//    }
//    catch (Exception e)
//    {
//        logger.Error("Falha ao tentar cumprimentar o servidor");
//        return Results.BadRequest(e.Message);
//    }
//    return Results.Ok("OK");

//});

//app.MapGet("/sale", [Trace] async () => {
//    try
//    {
//        var vendaId = r.Next();
//        var codigoProduto = r.Next();
//        var venda = new VendaDTO(vendaId, codigoProduto);

//        IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
//        ITransaction transaction = agent.CurrentTransaction;
//        transaction
//            .AddCustomAttribute("EventCode", 1)
//            .AddCustomAttribute("ProductCode", codigoProduto)
//            .AddCustomAttribute("SaleId", vendaId);

//        var client = new HttpClient();
//        var addItem = await client.PostAsync("http://hiperdev-api-rodrigo-service/addItemAsync", JsonContent.Create(venda));

//        var addPayment = await client.PostAsync("http://hiperdev-api-rodrigo-service/addPaymentAsync", JsonContent.Create(venda));

//        var closeSale = await client.PostAsync("http://hiperdev-api-rodrigo-service/completeSaleAsync", JsonContent.Create(venda));

//        var answer = closeSale.Content.ReadAsStringAsync().Result;
//        logger.Information($"Processamento da Venda: {answer}");
//    }
//    catch (Exception e)
//    {
//        logger.Error("Falha ao tentar cumprimentar o servidor");
//        return Results.BadRequest(e.Message);
//    }
//    return Results.Ok("OK");

//});

app.Run();


internal record VendaDTO
{
    public int VendaId { get; init; }
    public int CodigoProduto { get; init; }

    public VendaDTO(int vendaId, int codigoProduto)
    {
        VendaId = vendaId;
        CodigoProduto = codigoProduto;
    }
}