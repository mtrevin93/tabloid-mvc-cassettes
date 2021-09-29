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

        public void AddSubscription(Subscription subscription, int subscriber)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime)
                                        OUTPUT INSERTED.ID
                                        VALUES (@subscriber, @author, @beginDate, @endDate)";
                    cmd.Parameters.AddWithValue("@subscriber", subscriber);
                    cmd.Parameters.AddWithValue("@author", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDate", subscription.BeginDateTime);
                    cmd.Parameters.AddWithValue("@endDate", DbUtils.ValueOrDBNull(subscription.EndDateTime));

                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
