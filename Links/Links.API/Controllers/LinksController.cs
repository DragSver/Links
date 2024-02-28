using Links.BL;
using Links.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Links.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LinksController> _logger;

    public LinksController(IMediator mediator, ILogger<LinksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<string>> Get(Guid id)
    {
        var link = await _mediator.Send(new GetLinkRequest { Id = id });

        return link is not null ? Ok(link) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] AddLinkModel model)
    {
        var id = await _mediator.Send(new AddLinkRequest { Url = model.Url });

        return id is not null ? Ok(id) : NotFound();
    }
}