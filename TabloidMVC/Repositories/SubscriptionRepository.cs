using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using Microsoft.Data.SqlClient;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }

        public void AddSubscription(Subscription subscription)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime)
                                        OUTPUT INSERTED.ID
                                        VALUES (@subscriber, @author, @beginDate, @endDate)";
                    cmd.Parameters.AddWithValue("@subscriber", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@author", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDate", subscription.BeginDateTime);
                    cmd.Parameters.AddWithValue("@endDate", DBNull.Value);

                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public Subscription GetSubscriptionById(int currentUser, int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Subscription WHERE SubscriberUserProfileId = @subscriberUserProfileId AND ProviderUserProfileId = @providerUserProfileId";
                    cmd.Parameters.AddWithValue("@subscriberUserProfileId", currentUser);
                    cmd.Parameters.AddWithValue("@providerUserProfileId", authorId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Subscription subscription = null;

                    if (reader.Read())
                    {
                        subscription = new Subscription
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                            ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                            BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime"))
                        };
                    }
                    reader.Close();

                    return subscription;
                }
            }
        }

        public void Unsubscribe(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Subscription SET
                                        EndDateTime = @endDateTime
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@endDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
