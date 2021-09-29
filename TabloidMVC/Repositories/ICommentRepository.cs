using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        void DeleteComment(Comment comment);
        void Edit(Comment comment);
        Post GetPostByComment(Comment comment);
    }
}
