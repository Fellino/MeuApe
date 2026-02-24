namespace ApartmentPlanner.Api.Application.DTOs;

public class CreateApartmentRequest
{
    public string Name { get; set; }
    public int UserId { get; set; }
    public DateTime? DeliveredAt { get; set;
    }
}