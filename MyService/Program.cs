using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr(); 
builder.Services.AddDaprClient();   

//Bypass bug with OpenTelemetry and Dapr
// builder.Services.AddOpenTelemetry()
//     .WithTracing(b =>
//     {
//         b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("myservice"))
//          .AddAspNetCoreInstrumentation()
//          .AddHttpClientInstrumentation() 
//          .AddOtlpExporter(o =>
//          {
//              o.Endpoint = new System.Uri("http://localhost:4317");
//          });
//     });

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
//app.MapSubscribeHandler(); 

app.Run();
