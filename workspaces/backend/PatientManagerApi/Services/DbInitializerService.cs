using PatientManagerApi.Data;
using PatientManagerApi.Models;
using PatientManagerApi.Services;
using Microsoft.EntityFrameworkCore;

public class DbInitializerService
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;
    public DbInitializerService(ApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task InitializeAsync()
    {
        // Controlla se esistono già degli utenti
        if (!await _context.Users.AnyAsync())
        {
            // Crea utente Admin
            var adminUser = new User
            {
                Username = "admin",
                Role = "Admin"
            };

            adminUser.PasswordHash = _authService.HashPassword("admin", out string saltAdmin);
            adminUser.PasswordSalt = saltAdmin;

            // Crea utente standard
            var standardUser = new User
            {
                Username = "user",
                Role = "User"
            };

            standardUser.PasswordHash = _authService.HashPassword("user", out string saltUser);
            standardUser.PasswordSalt = saltUser;

            // Aggiungi gli utenti al database
            await _context.Users.AddRangeAsync(adminUser, standardUser);
            await _context.SaveChangesAsync();
        }
    }
} 