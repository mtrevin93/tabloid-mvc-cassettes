using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostListViewModel
    {
        public int UserId { get; set; }
        public List<Post> Posts { get; set; }
        public Post Post { get; set; }
    }
}
