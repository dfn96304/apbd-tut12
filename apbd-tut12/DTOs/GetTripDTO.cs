using apbd_tut12.Models;

namespace apbd_tut12.DTOs;

public class GetTripsDTO
{
    public ICollection<GetTripDTO> Trips { get; set; }
}

public class GetPaginatedTripsDTO : GetTripsDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
}

public class GetTripDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public ICollection<GetTripCountryDTO> Countries { get; set; }
    public ICollection<GetTripClientDTO> Clients { get; set; }
}

public class GetTripCountryDTO
{
    public string Name { get; set; }
}

public class GetTripClientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}