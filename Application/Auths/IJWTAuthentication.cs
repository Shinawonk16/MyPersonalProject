using Application.Dto;

namespace Application.Auths;

public interface IJWTAuthentication
{
    string GenerateToken(string key, string issuer, UserDto user);
    // static bool IsTokenValid(string key, string issuer, string token);
}
