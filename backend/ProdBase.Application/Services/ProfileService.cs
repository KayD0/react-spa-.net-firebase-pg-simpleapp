using ProdBase.Application.DTOs;
using ProdBase.Application.Interfaces;
using ProdBase.Domain.Entities;
using ProdBase.Domain.Interfaces;

namespace ProdBase.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public ProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<ProfileDto> GetProfileAsync(string firebaseUid)
        {
            var profile = await _userProfileRepository.GetByFirebaseUidAsync(firebaseUid);

            if (profile == null)
            {
                // Create a new profile if it doesn't exist
                profile = new UserProfile
                {
                    FirebaseUID = firebaseUid
                };

                profile = await _userProfileRepository.CreateAsync(profile);
            }

            return MapToDto(profile);
        }

        public async Task<ProfileDto> UpdateProfileAsync(string firebaseUid, ProfileUpdateRequest request)
        {
            var profile = await _userProfileRepository.GetByFirebaseUidAsync(firebaseUid);

            if (profile == null)
            {
                // Create a new profile if it doesn't exist
                profile = new UserProfile
                {
                    FirebaseUID = firebaseUid,
                    DisplayName = request.DisplayName,
                    Bio = request.Bio,
                    Location = request.Location,
                    Website = request.Website
                };

                profile = await _userProfileRepository.CreateAsync(profile);
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

                // Always update the UpdatedAt property with UTC time
                profile.CreatedAt = DateTime.SpecifyKind(profile.CreatedAt, DateTimeKind.Utc);
                profile.UpdatedAt = DateTime.UtcNow;

                profile = await _userProfileRepository.UpdateAsync(profile);
            }

            return MapToDto(profile);
        }

        private ProfileDto MapToDto(UserProfile profile)
        {
            return new ProfileDto
            {
                Id = profile.Id,
                FirebaseUID = profile.FirebaseUID,
                DisplayName = profile.DisplayName ?? "",
                Bio = profile.Bio ?? "",
                Location = profile.Location ?? "",
                Website = profile.Website ?? "",
                CreatedAt = profile.CreatedAt.ToString("o"),
                UpdatedAt = profile.UpdatedAt.ToString("o")
            };
        }
    }
}
