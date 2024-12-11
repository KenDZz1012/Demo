using Authorize.Repositories;
using Service.Lib.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<IAuthorizeRepository, AuthorizeRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseAuthorization();
app.MapControllers();
app.Run();
