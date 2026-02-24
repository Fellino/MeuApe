using ApartmentPlanner.Api.Domain.Entities;
using ApartmentPlanner.Api.Infrastructure.Data;
using ApartmentPlanner.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using ApartmentPlanner.Api.Application.DTOs;

namespace ApartmentPlanner.Api.Application.Services;

public class ApartmentService
{
    private readonly AppDbContext _context;

    public ApartmentService (AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateApartmentAsync(string name, int userId, DateTime? deliveredAt)
    {
        var apartment = new Apartment(name, userId, deliveredAt);

        _context.Apartments.Add(apartment);
        await _context.SaveChangesAsync(); // <- gerar o ID do apartamento salvando primeiro (para não ficar zerado)

        var member = new ApartmentMember(apartment.Id, userId, MemberRole.Owner);

        _context.ApartmentMembers.Add(member);
        await _context.SaveChangesAsync();

        return apartment.Id;
    }
    public async Task AddMemberAsync(int apartmentId, int ownerUserId, int newMemberUserId)
    {
        //valida se o apartamento existe
        var apartmentExists = await _context.Apartments.AnyAsync(ap => ap.Id == apartmentId);
        if (apartmentExists == false)
            throw new Exception("Apartamento não encontrado.");
        //valida se o usuário é proprietário do apartamento
        var isOwner = await _context.ApartmentMembers.AnyAsync(m => m.ApartmentId == apartmentId && m.UserId == ownerUserId && m.Role == MemberRole.Owner);
        if (isOwner == false)
            throw new Exception("Apenas o proprietário pode adicionar membros.");
        //valida se o usuário a ser adicionado existe
        var userExists = await _context.Users.AnyAsync(u => u.Id == newMemberUserId);
        if (userExists == false)
            throw new Exception("Usuário a ser adicionado não encontrado.");
        //valida se o usuário já é membro do apartamento
        var isMember = await _context.ApartmentMembers.AnyAsync(u => u.Id == newMemberUserId);
        if (isMember)
            throw new Exception("Usuário já é membro do apartamento.");

        var member = new ApartmentMember(apartmentId, newMemberUserId, MemberRole.Member);
        _context.ApartmentMembers.Add(member);

        await _context.SaveChangesAsync();
    }

    public async Task<List<MemberResponse>>
        GetMembersAsync(int apartmentId, int userId)
    {
        var isMember = await _context.ApartmentMembers.AnyAsync(m => m.ApartmentId == apartmentId && m.UserId == userId);
        if (isMember == false)
            throw new Exception("Usuário não pertence a este apartamento");

        var members = await _context.ApartmentMembers.Where(m => m.ApartmentId == apartmentId).Select(m => new MemberResponse
        {
            UserId = m.UserId,
            Name = m.User.Name,
            Email = m.User.Email,
            Role = m.Role.ToString()
        }).ToListAsync();

        return members;
    }

    public async Task<List<MyApartmentResponse>>
        GetApartmentsAsync(int userId)
    {
        var apartments = await _context.ApartmentMembers.Where(m => m.UserId == userId).Select(m => new MyApartmentResponse
        {
            ApartmentId = m.ApartmentId,
            Name = m.Apartment.Name,
            Role = m.Role.ToString()
        }).ToListAsync();
        return apartments;
    }
}
