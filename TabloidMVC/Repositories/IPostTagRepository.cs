using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostTagRepository
    {
        List<PostTag> GetAllPostTags(int id);
        void AddPostTag(PostTag postTag);

        void DeletePostTag(int id);
    }
}
