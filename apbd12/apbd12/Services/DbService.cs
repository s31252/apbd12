using apbd12.Data;
using apbd12.DTOs;
using apbd12.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd12.Services;

public class DbService : IDbService
{
    private readonly MasterContext _context;
    private IDbService _dbServiceImplementation;

    public DbService(MasterContext context)
    {
        _context = context;
    }
    
    public async Task<RequestPageDTO> GetPage(int pageNum, int pageSize)
    {
        if (pageNum <= 0) pageNum = 1;
        if (pageSize <= 0) pageSize = 10;
        var totalCount = await _context.Trips.CountAsync();

        
        var trips = await _context.Trips
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new RequestPageDTO
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = (int)Math.Ceiling(totalCount/(double)pageSize),
            Trips = trips.Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO
                {
                    Name = c.Name,
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName,
                }).ToList()
            }).ToList()
        };
    }

    public async Task<bool> DeleteClient(int id)
    {
        var client = await _context.Clients
            .Include(c=>c.ClientTrips)
            .FirstOrDefaultAsync(c=>c.IdClient == id);

        if (client==null)
        {
            throw new InvalidOperationException("Client not found");
        }

        if (client.ClientTrips.Any())
        {
            throw new InvalidOperationException("Client is assigned to at least one trip");
        }
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task AssignClientToTrip(int tripId,ClientTripRequestDto client)
    {
        var exists = await _context.Clients.FirstOrDefaultAsync(c=>c.Pesel==client.Pesel);
        if (exists!=null)
        {
            var assigned = await _context.ClientTrips
                .AnyAsync(ct=>ct.IdClient==exists.IdClient && ct.IdTrip==tripId);

            if (assigned)
                throw new InvalidOperationException("Client is assigned to a trip");
            throw new InvalidOperationException("Client with this Pesel already exists");
        }
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip==null || trip.DateFrom<=DateTime.Now)
            throw new InvalidOperationException("Trip does not found");

        var newClient = new Client
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Telephone = client.Telephone,
            Pesel = client.Pesel,
        };

        var clientsTrips = new ClientTrip
        {
            IdClientNavigation = newClient,
            IdTrip = tripId,
            RegisteredAt = DateTime.Now,
            PaymentDate = client.PaymentDate,
        };
        _context.Clients.Add(newClient);
        _context.ClientTrips.Add(clientsTrips);
        await _context.SaveChangesAsync();
    }
}