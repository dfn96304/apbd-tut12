using apbd_tut12.DTOs;
using apbd_tut12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut12.Controllers;

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
    public async Task<IActionResult> GetAllTrips()
    {
        var patients = await _dbService.GetAllTrips();
        return Ok(patients);
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip([FromBody] AssignClientToTripDTO assignClientToTripDto, int idTrip)
    {
        return Ok();
    }
}