using System.Data;
using System.Runtime.InteropServices.JavaScript;
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
        var client = await _context.Clients.Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);
        if (client == null)
        {
            throw new FileNotFoundException("Client with id " + idClient + " not found");
        }

        if (client.ClientTrips.Count > 0)
        {
            throw new ConstraintException("Client is assigned to " + client.ClientTrips.Count + " trips");
        }
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
    
    public async Task AssignClientToTrip(AssignClientToTripDTO assignClientToTripDto)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                bool clientAlreadyExists = _context.Clients.Any(c => c.Pesel == assignClientToTripDto.Pesel);
                if (clientAlreadyExists)
                {
                    throw new ConstraintException("A client with this PESEL already exists");
                }
                // 1 OK

                bool alreadyRegistered = _context.Clients.Any(c =>
                    c.Pesel == assignClientToTripDto.Pesel &&
                    c.ClientTrips.Any(ct => ct.IdTrip == assignClientToTripDto.IdTrip));
                if (alreadyRegistered)
                {
                    throw new ConstraintException("A Client with this PESEL is already registered for this Trip");
                }
                // 2 OK

                var trip = await _context.Trips.Include(t => t.ClientTrips)
                    .FirstOrDefaultAsync(t => t.IdTrip == assignClientToTripDto.IdTrip);
                if (trip == null)
                {
                    throw new FileNotFoundException("A Trip with the ID " + assignClientToTripDto.IdTrip +
                                                    " was not found");
                }

                var dateTimeNow = DateTime.Now;
                
                // the given trip exists
                if (trip.DateFrom <= dateTimeNow)
                {
                    throw new ConstraintException("This Trip has already started/occured");
                }
                // 3 OK

                var newClient = _context.Clients.Add(new Client()
                {
                    FirstName = assignClientToTripDto.FirstName,
                    LastName = assignClientToTripDto.LastName,
                    Email = assignClientToTripDto.Email,
                    Telephone = assignClientToTripDto.Telephone,
                    Pesel = assignClientToTripDto.Pesel,
                });

                _context.ClientTrips.Add(new ClientTrip()
                {
                    IdClient = newClient.Entity.IdClient,
                    IdTrip = assignClientToTripDto.IdTrip,
                    RegisteredAt = dateTimeNow,
                    PaymentDate = assignClientToTripDto.PaymentDate,
                    IdClientNavigation = newClient.Entity,
                    IdTripNavigation = trip
                });
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}