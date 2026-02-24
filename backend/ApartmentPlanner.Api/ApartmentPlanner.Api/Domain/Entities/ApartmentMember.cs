using ApartmentPlanner.Api.Domain.Enums;

namespace ApartmentPlanner.Api.Domain.Entities
{
    public class ApartmentMember
    {
        public int Id { get; private set; }
        public int ApartmentId { get; private set; }
        public int UserId { get; private set; }
        public MemberRole Role { get; private set; }
        public DateTime JoinedAt { get; private set; }

        public User User { get; private set; }
        public Apartment Apartment { get; private set; }

        private ApartmentMember() { } // <- usado no EF core (IMPORTANTE)

        public ApartmentMember(int apartmentId, int userId, MemberRole role)
        {
            if (apartmentId <= 0)
                throw new ArgumentException("ID do apartamento deve ser maior que zero.");
            if (userId <= 0)
                throw new ArgumentException("ID do usuário deve ser maior que zero.");
            if (!Enum.IsDefined(typeof(MemberRole), role))
                throw new ArgumentException("Role deve ser Owner ou Member.");

            ApartmentId = apartmentId;
            UserId = userId;
            Role = role;
            JoinedAt = DateTime.UtcNow;

        }
    }
}
