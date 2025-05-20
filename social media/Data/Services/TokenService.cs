using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using social_media.Data;
using social_media.Data.Models;
using social_media.Data.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;

    public TokenService(UserManager<User> userManager, IConfiguration config, IServiceScopeFactory scopeFactory)
    {
        _userManager = userManager;
        _config = config;
        _scopeFactory = scopeFactory;
    }

    public async Task<string> GenerateJWTTokenAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) throw new Exception("User not found");

        var jwtSettings = _config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expires = DateTime.UtcNow.AddMinutes(15);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var issuedAt = DateTime.UtcNow;
        var notBefore = issuedAt;

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            notBefore,
            expires: expires,           
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(int userId)
    {
        using var scope = _scopeFactory.CreateScope();

        var _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            IsExpired = false,
            ExpiredDate = DateTime.UtcNow.AddDays(7)
        };
        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();
        return token;
    }

    public async Task<List<string>> TryRefreshTokensAsync(string refreshToken)
    {
        using var scope = _scopeFactory.CreateScope();

        // گرفتن DbContext از Scope
        var _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

        if (string.IsNullOrEmpty(refreshToken))
            return null;

        var existingToken = _context.RefreshTokens
            .FirstOrDefault(rt => rt.Token == refreshToken && !rt.IsExpired && rt.ExpiredDate > DateTime.UtcNow);

        if (existingToken == null)
            return null;

        var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
        if (user == null)
            return null;

        // منقضی کردن ریفرش توکن فعلی
        //existingToken.IsExpired = true;
        //_context.RefreshTokens.Update(existingToken);
        //_context.SaveChanges();

        // تولید توکن های جدید
        var newAccessToken = await GenerateJWTTokenAsync(user.Id);
        //var newRefreshToken = GenerateRefreshToken(user.Id);

        List<string> tokens = new List<string>();

        tokens.Add(newAccessToken);
        tokens.Add(refreshToken);

        return  tokens;
    }
}
