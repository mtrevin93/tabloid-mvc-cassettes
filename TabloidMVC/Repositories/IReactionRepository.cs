using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IReactionRepository
    {
        public List<Reaction> Get();
        public void Add(int postId, int reactionId, int userProfileId);
        public int GetTimesUsed(int postId, int reactionId);
    }
}
