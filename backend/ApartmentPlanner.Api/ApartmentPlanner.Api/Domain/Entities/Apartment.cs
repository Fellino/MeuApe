namespace ApartmentPlanner.Api.Domain.Entities
{
    public class Apartment
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int CreatedByUserId { get; private set; }
        public DateTime? DeliveredAt { get; private set; }

        private Apartment() { } // <- usado no EF core (IMPORTANTE)
        public Apartment(string name, int createdByUserId, DateTime? deliveredAt)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome do apartamento não pode ser vazio.");
            if (createdByUserId <= 0)
                throw new ArgumentException("ID do usuário criador deve ser maior que zero.");
            
            Name = name;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.UtcNow;
            DeliveredAt = deliveredAt;

        }
    }
}
