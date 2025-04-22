using Microsoft.EntityFrameworkCore;
using ProdBase.Domain.Entities;
using ProdBase.Domain.Interfaces;

namespace ProdBase.Infrastructure.Data
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> GetByFirebaseUidAsync(string firebaseUid)
        {
            return await _dbContext.UserProfiles
                .FirstOrDefaultAsync(p => p.FirebaseUID == firebaseUid);
        }

        public async Task<UserProfile> CreateAsync(UserProfile userProfile)
        {
            // Ensure both DateTime properties are explicitly set to UTC time
            userProfile.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            userProfile.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            _dbContext.UserProfiles.Add(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }

        public async Task<UserProfile> UpdateAsync(UserProfile userProfile)
        {
            // Ensure UpdatedAt is explicitly set to UTC time
            userProfile.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            
            _dbContext.UserProfiles.Update(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }
    }
}
