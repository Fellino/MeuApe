using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApartmentPlanner.Api.Application.Services;
using ApartmentPlanner.Api.Application.DTOs;

namespace ApartmentPlanner.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return StatusCode(201);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }
}
