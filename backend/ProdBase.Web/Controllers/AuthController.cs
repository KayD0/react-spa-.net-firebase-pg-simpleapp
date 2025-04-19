using Microsoft.AspNetCore.Mvc;
using ProdBase.Web.Middleware;

namespace ProdBase.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("verify")]
        public IActionResult VerifyAuth()
        {
            try
            {
                // Get the user claims from the HttpContext (set by the AuthMiddleware)
                if (HttpContext.Items["UserClaims"] is not Dictionary<string, object> claims)
                {
                    return Unauthorized(new { error = "認証されていません" });
                }

                // Extract user info from the token
                claims.TryGetValue("uid", out var uid);
                claims.TryGetValue("email", out var email);
                claims.TryGetValue("email_verified", out var emailVerified);
                claims.TryGetValue("auth_time", out var authTime);

                // Return user info
                return Ok(new
                {
                    authenticated = true,
                    user = new
                    {
                        uid,
                        email,
                        email_verified = emailVerified,
                        auth_time = authTime
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VerifyAuth");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
