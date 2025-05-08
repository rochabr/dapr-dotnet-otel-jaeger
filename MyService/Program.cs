using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr(); 
builder.Services.AddDaprClient();            

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
//app.MapSubscribeHandler(); 

app.Run();
