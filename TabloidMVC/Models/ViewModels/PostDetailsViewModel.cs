using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public int CurrentUserId { get; set; }
        public int PostId { get; set; }
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
    }
}
