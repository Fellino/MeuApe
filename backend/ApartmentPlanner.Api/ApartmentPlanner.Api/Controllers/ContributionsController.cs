using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApartmentPlanner.Api.Application.Services;
using ApartmentPlanner.Api.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApartmentPlanner.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ContributionsController : ControllerBase
{
    private readonly ContributionService _contributionService;
    public ContributionsController(ContributionService contributionService)
    {
        _contributionService = contributionService;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateContributionRequest request)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            return Unauthorized();
        var userId = int.Parse(claim.Value);
        
        await _contributionService.CreateContributionAsync(request, userId);
        return StatusCode(201);
    }
}
