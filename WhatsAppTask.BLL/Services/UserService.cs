using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace WhatsAppTask.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User? Login(string usernameOrEmail, string password)
        {
            usernameOrEmail = usernameOrEmail.ToLower();
            var hashedPassword = HashPassword(password);

            return _context.Users.FirstOrDefault(u =>
                (u.Email == usernameOrEmail || u.Username == usernameOrEmail)
                && u.PasswordHash == hashedPassword
                && u.IsActive
            );
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        public User CreateUser(string username, string email, string password, string role)
        {
            username = username.ToLower();
            email = email.ToLower();

            if (_context.Users.Any(u => u.Username == username))
                throw new Exception("Username already exists");

            if (_context.Users.Any(u => u.Email == email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
