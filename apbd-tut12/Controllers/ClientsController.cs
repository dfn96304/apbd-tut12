using System.Data;
using apbd_tut12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut12.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public ClientsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _dbService.RemoveClient(idClient);
            return NoContent();
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConstraintException e)
        {
            return Conflict(e.Message);
        }
    }
}