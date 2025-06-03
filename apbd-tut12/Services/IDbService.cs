using apbd_tut12.DTOs;

namespace apbd_tut12.Services;

public interface IDbService
{
    public Task<GetTripsDTO> GetAllTrips(int? page, int? pageSize);
    public Task RemoveClient(int idClient);
    public Task AssignClientToTrip(AssignClientToTripDTO assignClientToTripDto);
}