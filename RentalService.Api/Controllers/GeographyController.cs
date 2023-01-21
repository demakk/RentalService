using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.GeographyContracts.Requests;

namespace RentalService.Api.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class GeographyController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCountry([FromBody] CountryCreate country)
    {
        return NoContent();
    }
}