using Microsoft.AspNetCore.Mvc;
using ProdBase.Application.DTOs;
using ProdBase.Application.Interfaces;
using ProdBase.Web.Middleware;

namespace ProdBase.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Get the user ID from the token
                var firebaseUID = HttpContext.GetUserIdFromToken();

                // Get the profile
                var profile = await _profileService.GetProfileAsync(firebaseUID);

                // Return the profile
                return Ok(new
                {
                    success = true,
                    profile
                });
            }
            catch (AuthError ex)
            {
                return Unauthorized(new
                {
                    success = false,
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProfile");
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateRequest request)
        {
            try
            {
                // Get the user ID from the token
                var firebaseUID = HttpContext.GetUserIdFromToken();

                // Update the profile
                var profile = await _profileService.UpdateProfileAsync(firebaseUID, request);

                // Return the updated profile
                return Ok(new
                {
                    success = true,
                    profile,
                    message = "プロフィールが更新されました"
                });
            }
            catch (AuthError ex)
            {
                return Unauthorized(new
                {
                    success = false,
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateProfile");
                return StatusCode(500, new
                {
                    success = false,
                    error = "プロフィールの更新に失敗しました: " + ex.Message
                });
            }
        }
    }
}
