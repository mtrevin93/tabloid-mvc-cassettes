using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ISubscriptionRepository
    {
        void AddSubscription(Subscription subscription);
        void Unsubscribe(int id);
        Subscription GetSubscriptionById(int currentUserId, int authorId);

        List<Subscription> GetActiveSubscriptions(int currentUser);
    }
}
