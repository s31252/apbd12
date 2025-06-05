using System.ComponentModel.DataAnnotations;

namespace apbd12.DTOs;

public class ClientTripRequestDto
{
    public string FirstName { get; set; }=String.Empty;
    public string LastName { get; set; }=String.Empty;
    public string Email { get; set; }=String.Empty;
    public string Telephone { get; set; }=String.Empty;
    [Length(11,11)]
    public string Pesel { get; set; }=String.Empty;
    
    public int IdTrip { get; set; }
    public string TripName { get; set; }=String.Empty;
    public DateTime? PaymentDate { get; set; }
}
