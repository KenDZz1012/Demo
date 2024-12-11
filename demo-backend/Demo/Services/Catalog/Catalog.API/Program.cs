using Catalog.API.Repositories;
using Microsoft.Data.SqlClient;
using Service.Lib.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<ICatalogRepository,CatalogRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();
