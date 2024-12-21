using Microsoft.AspNetCore.Mvc;
using PatientManagerApi.Data;
using PatientManagerApi.Models;
using PatientManagerApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace PatientManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;

    public TestController(ApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("test-data")]
    public async Task<IActionResult> CreateTestData()
    {
        try
        {
            // Verifica se l'utente admin esiste già
            var existingAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
            if (existingAdmin == null)
            {
                // Crea un utente admin di test
                var user = new User 
                { 
                    Username = "admin",
                    Role = "Admin"
                };
                
                // Hash della password
                user.PasswordHash = _authService.HashPassword("test123", out string salt);
                user.PasswordSalt = salt;

                _context.Users.Add(user);
            }

            // Creo un paziente di test
            var patient = new Patient
            {
                FamilyName = "Rossi",
                GivenName = "Mario",
                BirthDate = new DateTime(1980, 1, 1),
                Sex = "M"
            };

            _context.Patients.Add(patient);

            // Creo alcuni parametri di test
            var parameters = new[]
            {
                new Parameter 
                { 
                    Name = "Pressione",
                    Value = "120/80",
                    Alarm = false,
                    PatientID = 1
                },
                new Parameter 
                { 
                    Name = "Temperatura",
                    Value = "36.5",
                    Alarm = false,
                    PatientID = 1
                }
            };

            _context.Parameters.AddRange(parameters);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Dati di test creati con successo" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("test-data")]
    public async Task<IActionResult> GetTestData()
    {
        var users = await _context.Users
            .Select(u => new { u.ID, u.Username, u.Role }) // Escludiamo i campi sensibili
            .ToListAsync();

        var patients = await _context.Patients
            .Include(p => p.Parameters)
            .ToListAsync();

        return Ok(new { users, patients });
    }
} 