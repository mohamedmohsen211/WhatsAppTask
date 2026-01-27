using System.Security.Cryptography;
using System.Text;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public User Create(string email, string password)
    {
        email = email.ToLower();

        if (_context.Users.Any(u => u.Email == email))
            throw new Exception("Email already exists");

        var user = new User
        {
            Username = email.Split('@')[0],
            Email = email,
            PasswordHash = PasswordHasher.Hash(password),
            Role = "admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public List<User> GetAll()
    {
        return _context.Users
            .Where(u => u.Role != null && u.Role.ToLower() == "admin")
            .ToList();
    }

    public User Update(int id, string email)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            throw new Exception("Admin not found");

        user.Email = email;
        _context.SaveChanges();
        return user;
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            throw new Exception("Admin not found");

        _context.Users.Remove(user);
        _context.SaveChanges();
    }
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
