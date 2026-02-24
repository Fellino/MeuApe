using ApartmentPlanner.Api.Application.DTOs;
using ApartmentPlanner.Api.Domain.Entities;
using ApartmentPlanner.Api.Domain.Enums;
using ApartmentPlanner.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ApartmentPlanner.Api.Application.Services;

public class ContributionService
{
    private readonly AppDbContext _context;
    public ContributionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateContributionAsync(CreateContributionRequest request, int userId)
    {
        var apartment = await _context.Apartments.FirstOrDefaultAsync(ap => ap.Id == request.ApartmentId);
        if (apartment == null)
        {
            throw new Exception("Apartamento não foi encontrado.");
        }
        var isMember = await _context.ApartmentMembers.AnyAsync(m => m.ApartmentId == request.ApartmentId && m.UserId == userId);
        if (isMember == false)
        {
            throw new Exception("usuario não é membro do apartamento.");
        }

        var type = (ContributionType)request.Type;
        if (!Enum.IsDefined(typeof(ContributionType), type))
            throw new Exception("Tipo de contribuição inválida.");
        if (type == ContributionType.Withdrawal)
        {
            var balance = await CalculateBalanceAsync(request.ApartmentId);
            if (request.Amount > balance)
                throw new Exception("Saldo insuficiente para realizar a retirada.");
        }

        var contribution = new Contribution(request.ApartmentId, userId, request.Amount, type);

        _context.Contributions.Add(contribution);
        await _context.SaveChangesAsync();
    }
    private async Task<decimal> CalculateBalanceAsync(int apartmentId)
    {
        var totalDeposits = await _context.Contributions.Where(c => c.ApartmentId == apartmentId && c.Type == ContributionType.Deposit)
            .SumAsync(c => c.Amount);
        var totalWithdrawals = await _context.Contributions.Where(c => c.ApartmentId == apartmentId && c.Type == ContributionType.Withdrawal)
            .SumAsync(c => c.Amount);
        return totalDeposits - totalWithdrawals;
    }

    private async Task ValidateMemberAsync(int apartmentId, int userId)
    {
        var isMember = await _context.ApartmentMembers
            .AnyAsync(m => m.ApartmentId == apartmentId && m.UserId == userId);

        if (isMember == false)
            throw new Exception("Usuario não pertence ao apartamento.");
    }
    public async Task<decimal> GetBalanceAsync(int apartmentId, int userId)
    {
        var apartment = await _context.Apartments.FirstOrDefaultAsync(ap => ap.Id == apartmentId);
        if (apartment == null)
        {
            throw new Exception("Apartamento não foi encontrado.");
        }
        await ValidateMemberAsync(apartmentId, userId);
        return await CalculateBalanceAsync(apartmentId);
    }

    public async Task<List<ContributionResponse>> GetContributionsAsync(int apartmentId, int userId)
    {
        await ValidateMemberAsync(apartmentId, userId);

        var apartmentExists = await _context.Apartments.AnyAsync(ap => ap.Id == apartmentId);
        if (apartmentExists == false)
        {
            throw new Exception("Apartamento não foi encontrado.");
        }

        var contributions = await _context.Contributions.Where(c => c.ApartmentId == apartmentId).OrderByDescending(c => c.Date).Select(c => new ContributionResponse
        {
            Id = c.Id,
            Amount = c.Amount,
            Type = c.Type.ToString(),
            Date = c.Date
        }).ToListAsync();

        return contributions;
    }
}
