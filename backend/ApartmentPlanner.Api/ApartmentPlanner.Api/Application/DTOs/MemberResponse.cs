using System.Reflection.Metadata;

namespace ApartmentPlanner.Api.Application.DTOs;

public class MemberResponse
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
