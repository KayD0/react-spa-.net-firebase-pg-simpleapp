using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdBase.Web.Data;
using ProdBase.Web.Middleware;
using ProdBase.Web.Models;
using ProdBase.Web.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProdBase.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext dbContext, ILogger<ProfileController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Get the user ID from the token
                var firebaseUID = HttpContext.GetUserIdFromToken();

                // Find the user profile
                var profile = _dbContext.UserProfiles.Where(x => x.FirebaseUID == firebaseUID).First();
                //    .FirstOrDefaultAsync(p => p.FirebaseUID == firebaseUID);

                if (profile == null)
                {
                    // Profile not found, create a new one
                    profile = new UserProfile
                    {
                        FirebaseUID = firebaseUID
                    };

                    _dbContext.UserProfiles.Add(profile);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new
                    {
                        success = true,
                        profile = profile.ToMap(),
                        message = "プロフィールが作成されました"
                    });
                }

                // Return the profile
                return Ok(new
                {
                    success = true,
                    profile = profile.ToMap()
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

                // Find the user profile
                var profile = await _dbContext.UserProfiles
                    .FirstOrDefaultAsync(p => p.FirebaseUID == firebaseUID);

                if (profile == null)
                {
                    // Profile not found, create a new one
                    profile = new UserProfile
                    {
                        FirebaseUID = firebaseUID,
                        DisplayName = request.DisplayName,
                        Bio = request.Bio,
                        Location = request.Location,
                        Website = request.Website
                    };

                    _dbContext.UserProfiles.Add(profile);
                }
                else
                {
                    // Update the profile
                    if (!string.IsNullOrEmpty(request.DisplayName))
                    {
                        profile.DisplayName = request.DisplayName;
                    }
                    if (!string.IsNullOrEmpty(request.Bio))
                    {
                        profile.Bio = request.Bio;
                    }
                    if (!string.IsNullOrEmpty(request.Location))
                    {
                        profile.Location = request.Location;
                    }
                    if (!string.IsNullOrEmpty(request.Website))
                    {
                        profile.Website = request.Website;
                    }
                }

                await _dbContext.SaveChangesAsync();

                // Return the updated profile
                return Ok(new
                {
                    success = true,
                    profile = profile.ToMap(),
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

    public class ProfileUpdateRequest
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
        
        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }
    }
}
