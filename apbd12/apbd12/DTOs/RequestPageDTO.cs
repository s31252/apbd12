namespace apbd12.DTOs;

public class RequestPageDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDTO> Trips { get; set; } = [];
}

public class TripDTO
{
    public string Name { get; set; }=String.Empty;
    public string Description{get;set;}=String.Empty;
    public DateTime DateFrom {get;set;}
    public DateTime DateTo {get;set;}
    public int MaxPeople {get;set;}
    public List<CountryDTO> Countries { get; set; } = [];
    public List<ClientDTO> Clients { get; set; } = [];
}

public class CountryDTO
{
    public string Name { get; set; }
}

public class ClientDTO
{
    public string FirstName { get; set; }=String.Empty;
    public string LastName { get; set; }=String.Empty;
}