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
                                      Content = @content
                                      WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", comment.Id);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Subject, Content, PostId, UserProfileId, cm.CreateDateTime, up.Id AS AuthorId, up.DisplayName AS CommentAuthor
                       FROM Comment cm
                       LEFT JOIN UserProfile up ON cm.UserProfileId = up.Id
                       WHERE cm.id=@id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Comment comment = new Comment();
                    if (reader.Read())
                    {
                            comment.Id = id;
                            comment.Content = reader.GetString(reader.GetOrdinal("Content"));
                            comment.Subject = reader.GetString(reader.GetOrdinal("Subject"));
                            comment.CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"));
                            comment.PostId = reader.GetInt32(reader.GetOrdinal("PostId"));
                            comment.Author = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                DisplayName = reader.GetString(reader.GetOrdinal("CommentAuthor"))
                            };
                    }

                    reader.Close();
                    return comment;
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
