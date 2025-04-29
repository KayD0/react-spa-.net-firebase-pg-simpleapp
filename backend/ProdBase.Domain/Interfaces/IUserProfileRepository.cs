using ProdBase.Domain.Entities;

namespace ProdBase.Domain.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetByFirebaseUidAsync(string firebaseUid);
        Task<UserProfile> CreateAsync(UserProfile userProfile);
        Task<UserProfile> UpdateAsync(UserProfile userProfile);
    }
}
