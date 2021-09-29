using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using Microsoft.Data.SqlClient;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public UserProfile GetByEmail(string email)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }






































        public List<UserProfile> GetAllUsers()
        {
            using (SqlConnection conn = Connection)
            {
                List<UserProfile> users = new List<UserProfile>();
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                                    u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                                    ut.[Name] AS UserTypeName
                                FROM UserProfile u
                                LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                                WHERE u.UserTypeId <> 3 AND u.UserTypeId <> 4
                                ORDER BY DisplayName
                                ";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProfile userProfile = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                }
                            };
                            users.Add(userProfile);
                        }
                    }
                    return users;
                }
            }
        }

        public List<UserProfile> GetDeactivatedUsers()
        {
            using (SqlConnection conn = Connection)
            {
                List<UserProfile> users = new List<UserProfile>();
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                                    u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                                    ut.[Name] AS UserTypeName
                                FROM UserProfile u
                                LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                                WHERE u.UserTypeId = 3 OR u.UserTypeId = 4
                                ORDER BY DisplayName
                                ";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProfile userProfile = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                }
                            };
                            users.Add(userProfile);
                        }
                    }
                    return users;
                }
            }
        }

        public UserProfile GetUserProfileById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                            u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                            ut.[Name] AS UserTypeName
                        FROM UserProfile u
                        LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE u.id = @id        
                        ";
                    cmd.Parameters.AddWithValue("@id", id);
                    UserProfile userProfile = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (userProfile == null)
                            {
                                userProfile = new UserProfile
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                    ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    UserType = new UserType()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                        Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                    }
                                };
                            }
                        }
                    }
                    return userProfile;
                }
            }
        }

        public void DeactivateAdminProfile(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Update UserProfile
                        SET UserTypeId = 3
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void DeactivateAuthorProfile(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Update UserProfile
                        SET UserTypeId = 4
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void ReactivateAuthorProfile(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Update UserProfile
                        SET UserTypeId = 2
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void ReactivateAdminProfile(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Update UserProfile
                        SET UserTypeId = 1
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Update UserProfile
                        SET
                            UserTypeId = @userTypeId
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List <UserType> GetAllUserTypes()
        {
            using (SqlConnection conn = Connection)
            {
                List<UserType> UserTypes = new List<UserType>();
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT Id , Name 
                                FROM UserType
                                ";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserType> userTypes = new List<UserType>();
                        
                        while (reader.Read())
                        {
                            UserType userType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                
                            };
                            userTypes.Add(userType);
                        }
                    return userTypes;
                    }
                }
            }
        }

    }
}
