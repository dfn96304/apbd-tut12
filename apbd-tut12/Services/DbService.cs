using apbd_tut12.Data;
using apbd_tut12.DTOs;
using Microsoft.EntityFrameworkCore;

namespace apbd_tut12.Services;

public class DbService : IDbService
{
    private readonly MasterContext _context;

    public DbService(MasterContext context)
    {
        _context = context;
    }

    public async Task<List<GetTripDTO>> GetAllTrips()
    {
        var trips = await _context.Trips.Select(e => new GetTripDTO()
        {
            Name = e.Name,
            Description = e.Description,
            DateFrom = e.DateFrom,
            DateTo = e.DateTo,
            MaxPeople = e.MaxPeople,
            Countries = e.IdCountries.Select(co => new GetTripCountryDTO()
            {
                Name = co.Name,
            }).ToList(),
            Clients = e.ClientTrips.Select(ct => new GetTripClientDTO()
            {
                FirstName = ct.IdClientNavigation.FirstName,
                LastName = ct.IdClientNavigation.LastName,
            }).ToList(),
        }).ToListAsync();

        return trips;
    }
    
    
}