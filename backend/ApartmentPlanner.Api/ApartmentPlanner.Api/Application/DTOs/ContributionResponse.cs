namespace ApartmentPlanner.Api.Application.DTOs;

public class ContributionResponse
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    }
