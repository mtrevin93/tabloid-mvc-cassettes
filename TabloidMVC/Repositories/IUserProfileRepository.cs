using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        List<UserProfile> GetAllUsers();

        List<UserType> GetAllUserTypes();
        List<UserProfile> GetDeactivatedUsers();
        UserProfile GetUserProfileById(int id);

        void DeactivateAuthorProfile(UserProfile userProfile);
        void DeactivateAdminProfile(UserProfile userProfile);

        void ReactivateAuthorProfile(UserProfile userProfile);
        void ReactivateAdminProfile(UserProfile userProfile);
        void Update(UserProfile userProfile);

    }
}