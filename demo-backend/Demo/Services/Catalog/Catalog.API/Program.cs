using Catalog.API.Interface;
using Catalog.API.Repositories;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Service.Lib.Context;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console() // Optional: Logs to Console
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "logs-{0:yyyy.MM.dd}"
    })
    .CreateLogger();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<ITestCodeRepository, TestCodeRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
