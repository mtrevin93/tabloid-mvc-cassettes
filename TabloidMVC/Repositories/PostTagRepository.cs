using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository, IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }

        public void AddPostTag(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostTag (TagId, PostId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@tagId, @postId)";
                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);
                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);

                    postTag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void DeletePostTag(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM PostTag WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PostTag> GetAllPostTags(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.Id AS PostTagId, PostId, TagId, t.Id AS TagsId, Name 
                                        FROM PostTag pt
                                        JOIN Tag t ON pt.TagId = t.Id
                                        WHERE PostId = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<PostTag> PostTags = new List<PostTag>();

                    while(reader.Read())
                    {
                        PostTag PostTag = new PostTag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("PostTagId")),
                            PostId = id,
                            Tag = new Tag
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                        
                        PostTags.Add(PostTag);
                    }
                    reader.Close();
                    return PostTags;
                }
            }
        }
    }
}
