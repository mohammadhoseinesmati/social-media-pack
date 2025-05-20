namespace social_media.Data.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJWTTokenAsync(int userId);
        string GenerateRefreshToken(int userId);
        Task<List<string>> TryRefreshTokensAsync(string refreshToken);
    }
}
