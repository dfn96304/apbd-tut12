using System.Data;
using apbd_tut12.DTOs;
using apbd_tut12.Exceptions;
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
    public async Task<IActionResult> GetAllTrips([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        try
        {
            var patients = await _dbService.GetAllTrips(page, pageSize);
            return Ok(patients);
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip([FromBody] AssignClientToTripDTO assignClientToTripDto)
    {
        try
        {
            await _dbService.AssignClientToTrip(assignClientToTripDto);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }

        return Ok();
    }
}