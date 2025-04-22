using Microsoft.EntityFrameworkCore;
using ProdBase.Domain.Entities;
using ProdBase.Domain.Interfaces;
using System.Threading.Tasks;

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
            _dbContext.UserProfiles.Add(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }

        public async Task<UserProfile> UpdateAsync(UserProfile userProfile)
        {
            _dbContext.UserProfiles.Update(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }
    }
}
