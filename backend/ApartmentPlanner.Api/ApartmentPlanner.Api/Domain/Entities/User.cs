namespace ApartmentPlanner.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private User() { } // <- usado no EF core (IMPORTANTE)

        public User(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Senha não pode ser vazia.");

            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
