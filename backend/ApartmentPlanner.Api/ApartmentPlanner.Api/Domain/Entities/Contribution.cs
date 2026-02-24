using ApartmentPlanner.Api.Domain.Enums;

namespace ApartmentPlanner.Api.Domain.Entities
{
    public class Contribution
    {
        public int Id { get; private set; }
        public int ApartmentId { get; private set; }
        public int UserId { get; private set; }
        public decimal Amount { get; private set; }
        public ContributionType Type { get; private set; }
        public DateTime Date { get; private set; }

        private Contribution() { } // <- usado no EF core (IMPORTANTE)

        public Contribution(int apartmentId, int userId, decimal amount, ContributionType type)
        {
            if (apartmentId <= 0)
                throw new ArgumentException("ID do apartamento deve ser maior que zero.");
            if (userId <= 0)
                throw new ArgumentException("ID do usuário deve ser maior que zero.");
            if (amount <= 0)
                throw new ArgumentException("Valor do depósito ou saque deve ser maior que zero.");
            if (!Enum.IsDefined(typeof(ContributionType), type))
                throw new ArgumentException("Tipo de contribuição deve ser Deposit ou Withdrawal.");

            ApartmentId = apartmentId;
            UserId = userId;
            Amount = amount;
            Type = type;
            Date = DateTime.UtcNow;
        }
    }
}
