using apbd12.DTOs;

namespace apbd12.Services;

public interface IDbService
{
    Task<RequestPageDTO> GetPage(int pageNum, int pageSize);
    Task<bool> DeleteClient(int id);
    
    Task AssignClientToTrip(int tripId,ClientTripRequestDto client);
}