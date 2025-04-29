using ProdBase.Application.DTOs;

namespace ProdBase.Application.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileAsync(string firebaseUid);
        Task<ProfileDto> UpdateProfileAsync(string firebaseUid, ProfileUpdateRequest request);
    }
}
