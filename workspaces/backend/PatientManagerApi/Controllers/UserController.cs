using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagerApi.Data;
using PatientManagerApi.Models;
using PatientManagerApi.Models.DTOs;
using PatientManagerApi.Services;

namespace PatientManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;

    public UserController(ApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    // GET: /User
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        var response = users.Select(u => new UserResponse
        {
            ID = u.ID,
            Username = u.Username,
            Role = u.Role
        }).ToList();

        return Ok(response);
    }

    // GET: /User/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var response = new UserResponse
        {
            ID = user.ID,
            Username = user.Username,
            Role = user.Role
        };

        return Ok(response);
    }

    // POST: /User
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request)
    {
        // Verifica se l'username esiste già
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest("Username già in uso");
        }

        // Crea il nuovo utente con password hashata
        var user = new User
        {
            Username = request.Username,
            Role = request.Role
        };

        // Hash della password
        user.PasswordHash = _authService.HashPassword(request.Password, out string salt);
        user.PasswordSalt = salt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var response = new UserResponse
        {
            ID = user.ID,
            Username = user.Username,
            Role = user.Role
        };

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.ID },
            response
        );
    }

    // PUT: /User/5
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, CreateUserRequest request)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Verifica se il nuovo username è già in uso da un altro utente
        if (await _context.Users.AnyAsync(u => u.Username == request.Username && u.ID != id))
        {
            return BadRequest("Username già in uso");
        }

        // Aggiorna i campi
        user.Username = request.Username;
        user.Role = request.Role;

        // Se è stata fornita una nuova password, aggiorna l'hash
        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = _authService.HashPassword(request.Password, out string salt);
            user.PasswordSalt = salt;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Users.AnyAsync(u => u.ID == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: /User/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Non permettere l'eliminazione dell'ultimo admin
        if (user.Role == "Admin" && await _context.Users.CountAsync(u => u.Role == "Admin") <= 1)
        {
            return BadRequest("Non è possibile eliminare l'ultimo amministratore");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 