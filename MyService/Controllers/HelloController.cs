using Microsoft.AspNetCore.Mvc;

namespace MyService.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello from .NET + Dapr + OTEL!";
    }
}
