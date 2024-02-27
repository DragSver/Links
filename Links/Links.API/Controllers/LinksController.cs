using Microsoft.AspNetCore.Mvc;

namespace Links.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;

    public LinksController(ILogger<LinksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<string> Get() => Ok("Hello World!");
}
