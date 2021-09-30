using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        public void AddUser(UserProfile userProfile);

        public List<UserProfile> GetAllUsers();
        public List<UserProfile> GetDeactivatedUsers();

        public UserProfile GetUserProfileById(int id);

        public void DeactivateAdminProfile(UserProfile userProfile);

        public void DeactivateAuthorProfile(UserProfile userProfile);

        public void ReactivateAuthorProfile(UserProfile userProfile);

        public void ReactivateAdminProfile(UserProfile userProfile);

        public void Update(UserProfile userProfile);

        public List<UserType> GetAllUserTypes();

    }
}