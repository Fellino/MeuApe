namespace ApartmentPlanner.Api.Application.DTOs;

public class CreateContributionRequest
{
    public int ApartmentId { get; set; }
    public decimal Amount { get; set; }
    public int Type { get; set; } // <- é Int pq vai converter depois no service
}
