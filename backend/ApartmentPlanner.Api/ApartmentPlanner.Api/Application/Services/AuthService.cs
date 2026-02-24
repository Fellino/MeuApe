using ApartmentPlanner.Api.Domain.Entities;
using ApartmentPlanner.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ApartmentPlanner.Api.Application.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging.Abstractions;


namespace ApartmentPlanner.Api.Application.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
        _configuration = configuration;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        // Verificar se o email já está cadastrado
        var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (userExists)
            throw new Exception("Email já cadastrado");
        // Criar um usuário temporário para gerar o hash da senha
        var tempUser = new User(request.Name, request.Email, "temp");
        // Gerar o hash da senha usando o PasswordHasher
        var hash = _passwordHasher.HashPassword(tempUser, request.Password);
        var user = new User(request.Name, request.Email, hash);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
            throw new Exception("Usuário não encontrado.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Senha incorreta.");

        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Token = token
        };
    }
}
