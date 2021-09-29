using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public void Delete(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Comment WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Edit(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Comment SET
                                      Subject = @subject,
                                      Content = @content,
                                      PostId = @postId,
                                      UserProfileId = @userProfileId,
                                      CreateDateTime = @createDateTime,
                                      WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", comment.Id);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.Author.Id);


                    cmd.ExecuteNonQuery();

                }
            }
        }
        public Post GetPostByComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id AS PostId FROM Post p
                                        JOIN Comment c
                                        ON p.Id = c.PostId
                                        WHERE c.Id = @id";

                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;

                    if (reader.Read())
                    {
                        post = new Post { Id = reader.GetInt32(reader.GetOrdinal("PostId")) };
                    }
                    return post;          
                }
            }
        }
        public void Create(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            PostId, UserProfileId, Subject, Content, CreateDateTime)
                        VALUES (
                            @postId, @userProfileId, @subject, @content, @createDateTime)";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.Author.Id);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
