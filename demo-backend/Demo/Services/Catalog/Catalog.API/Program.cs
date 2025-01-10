using Catalog.API.Interface;
using Catalog.API.Repositories;
using Microsoft.Data.SqlClient;
using Service.Lib.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<ITestCodeRepository, TestCodeRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
