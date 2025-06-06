using apbd12.DTOs;
using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<ActionResult> GetPages([FromQuery]int page=1, [FromQuery]int pageSize=10)
    {
        var result =await  _dbService.GetPage(page,pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<ActionResult> GetClients(int idTrip,ClientTripRequestDto dto)
    {
        try
        {
            await _dbService.AssignClientToTrip(idTrip,dto);
            return Ok($"Client {dto.FirstName} {dto.LastName} assigned to trip {idTrip} successfully");
        }
        catch (Exception e)
        {
           return BadRequest(e.Message);
        }
    }
}