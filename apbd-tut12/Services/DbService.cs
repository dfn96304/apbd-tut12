using System.Data;
using apbd_tut12.Data;
using apbd_tut12.DTOs;
using apbd_tut12.Models;
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

    public async Task RemoveClient(int idClient)
    {
        Client? client = await _context.Clients.Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);
        if (client == null)
        {
            throw new FileNotFoundException("Client with id " + idClient + " not found");
        }

        if (client.ClientTrips.Count > 0)
        {
            throw new ConstraintException("Client is assigned to " + client.ClientTrips.Count + " trips");
        }

        Console.WriteLine(client.ClientTrips.Count);
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}