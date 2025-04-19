using Microsoft.AspNetCore.Http;
using ProdBase.Web.Services;
using System.Security.Claims;

namespace ProdBase.Web.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, FirebaseAuthService firebaseAuthService)
        {
            // Get the Authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Authorizationヘッダーがありません" });
                return;
            }

            // Extract the token (remove 'Bearer ' prefix if present)
            var idToken = authHeader;
            if (idToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                idToken = idToken.Substring(7);
            }

            try
            {
                // Verify the token
                var claims = await firebaseAuthService.VerifyIdTokenAsync(idToken);

                // Create a ClaimsPrincipal and set it on the HttpContext
                var claimsIdentity = new ClaimsIdentity(
                    claims.Select(c => new Claim(c.Key, c.Value?.ToString() ?? "")),
                    "Firebase");

                context.User = new ClaimsPrincipal(claimsIdentity);

                // Store the token claims in the HttpContext.Items for later use
                context.Items["UserClaims"] = claims;

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = $"無効な認証トークンです: {ex.Message}" });
            }
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirebaseAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }

    public static class HttpContextExtensions
    {
        public static string GetUserIdFromToken(this HttpContext context)
        {
            if (context.Items["UserClaims"] is not Dictionary<string, object> claims)
            {
                throw new AuthError("認証されていません。AuthMiddlewareを使用してください。");
            }

            if (!claims.TryGetValue("user_id", out var uid) || uid is not string userId)
            {
                throw new AuthError("ユーザーIDが見つかりません");
            }

            return userId;
        }
    }
}
