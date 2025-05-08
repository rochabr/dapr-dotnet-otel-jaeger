using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace MyService.Controllers;

[ApiController]
[Route("[controller]")]
public class StateController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public StateController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpPost("{key}")]
    public async Task<IActionResult> SaveState(string key, [FromBody] object value)
    {
        await _daprClient.SaveStateAsync("statestore", key, value);
        return Ok();
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetState(string key)
    {
        var value = await _daprClient.GetStateAsync<object>("statestore", key);
        return value is not null ? Ok(value) : NotFound();
    }
}