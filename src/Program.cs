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

app.MapGet("/lazy", () =>
{
    var x = 0;
    try
    {
        var z = 10;
        var y = 20;
        Thread.Sleep(5000);
        x = z * y;
    }
    catch (Exception e)
    {
        logger.Error($"Erro ao executar a função: {e.Message}");
        return Results.BadRequest("Not OK");
    }
    return Results.Ok($"O resultado da função é: {x}");
});

app.MapGet("/greetAnotherServer", async () => {
    try
    {
        var client = new HttpClient();
        var response = await client.GetAsync("http://hiperdev-api-rodrigo-service/hello");
        var answer = response.Content.ReadAsStringAsync().Result;

        logger.Information($"O servidor respondeu: {answer}");
    }
    catch (Exception e)
    {
        logger.Error("Falha ao tentar cumprimentar o servidor");
        return Results.BadRequest(e.Message);
    }
    return Results.Ok("OK");

});

app.Run();