using apbd_tut12.DTOs;

namespace apbd_tut12.Services;

public interface IDbService
{
    public Task<List<GetTripDTO>> GetAllTrips();
    public Task RemoveClient(int idClient);
}