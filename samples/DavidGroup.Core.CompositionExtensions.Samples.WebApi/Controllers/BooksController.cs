using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace DavidGroup.Core.CompositionExtensions.Samples.WebApi.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult All()
    {
        string[] books =
        [
            "The Shining",
            "Needful Things",
            "IT",
            "Misery",
            "Doctor Sleep",
            "Cell",
            "The Institute",
            "Mr. Mercedes",
            "The Green Mile"
        ];

        return Ok(books);
    }
}
