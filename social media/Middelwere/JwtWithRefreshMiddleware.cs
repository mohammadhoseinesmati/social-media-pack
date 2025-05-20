using Microsoft.AspNetCore.Mvc;
using social_media.Data.Services;
using System.IdentityModel.Tokens.Jwt;

public class JwtWithRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    public JwtWithRefreshMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Invoke(HttpContext context)
    {

        if (context.Request.Path.StartsWithSegments("/authentication", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }
        using var scope = _scopeFactory.CreateScope();
        var _tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var accessToken = context.Request.Cookies["AccessToken"];
        var refreshToken = context.Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
        {

            var newTokens = await _tokenService.TryRefreshTokensAsync(refreshToken);

            if (newTokens != null)
            {

                context.Response.Cookies.Append("AccessToken", newTokens[0], new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddSeconds(10)
                });

                context.Response.Cookies.Append("RefreshToken", newTokens[1], new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
                var isApiRequest = context.Request.Headers["Accept"].ToString().Contains("application/json");
                if (isApiRequest)
                {
                    context.Response.StatusCode = 200;
                }
                else
                {
                    context.Response.Redirect(context.Request.Path + context.Request.QueryString);
                }
                return;

            }
        }
        else if(string.IsNullOrEmpty(refreshToken))
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Response.Cookies.Delete("AccessToken");
            }
            // رفرش توکن معتبر نیست، درخواست 401 برگردان
            context.Response.Redirect("/authentication/login/");
            return;
        }



        await _next(context);
    }
}
