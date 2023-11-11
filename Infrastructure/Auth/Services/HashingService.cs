using System.Security.Cryptography;
using System.Text;
using Application.Auth.Commands.Register;
using Domain.Auth.Interfaces;

namespace Infrastructure.Auth.Services;

public class HashingService : IHashingService
{
    
    public string Hash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value, 12);
    }

    public bool Verify(string value, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(value, hash);
    }

    public string GenerateRandomToken()
    {
        var guid = Guid.NewGuid();
        return Convert.ToBase64String(guid.ToByteArray());
    }
}