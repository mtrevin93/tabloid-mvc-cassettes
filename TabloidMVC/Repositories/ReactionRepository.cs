using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using Microsoft.Data.SqlClient;

namespace TabloidMVC.Repositories
{
    public class ReactionRepository : BaseRepository, IReactionRepository
    {
        public ReactionRepository(IConfiguration config) : base(config) { }
        public List<Reaction> Get()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Name, ImageLocation, Id
                        FROM Reaction
                        ";
                    var reader = cmd.ExecuteReader();

                    var reactions = new List<Reaction>();

                    while (reader.Read())
                    {
                        reactions.Add(new Reaction()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
                        });
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }
        public int GetTimesUsed(int postId, int reactionId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id FROM PostReaction
                                      WHERE PostId = @postId AND ReactionId = @reactionId
                        ";

                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@reactionId", reactionId);

                    var reader = cmd.ExecuteReader();

                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                    }

                    reader.Close();

                    return count;
                }
            }
        }
        public void Add(int postId, int reactionId, int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO PostReaction (
                            PostId, ReactionId, UserProfileId)
                        VALUES (
                            @postId, @reactionId, @userProfileId)";

                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@reactionId", reactionId);
                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
