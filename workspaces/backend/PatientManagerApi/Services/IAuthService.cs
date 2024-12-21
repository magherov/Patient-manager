using PatientManagerApi.Models;
using PatientManagerApi.Models.DTOs;

namespace PatientManagerApi.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    string GenerateJwtToken(User user);
    string HashPassword(string password, out string salt);
    bool VerifyPassword(string password, string hash, string salt);
} 